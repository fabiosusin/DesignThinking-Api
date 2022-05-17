using Business.API.Hub.Integration.Surf.Customer;
using Business.API.Hub.Integration.Surf.Subscription;
using DAO.DBConnection;
using DAO.General.Log;
using DAO.Mobile.Surf;
using DAO.Hub.AccountPlan;
using DAO.Hub.AllyDAO;
using DAO.Hub.Cellphone;
using DAO.Hub.Order;
using DAO.Hub.Product;
using DTO.General.Base.Api.Output;
using DTO.Hub.Cellphone.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTO.Hub.Cellphone.Output;
using DTO.Hub.Order.Output;
using DTO.Hub.Cellphone.Database;
using DAO.Hub.CustomerDAO;
using Business.API.Hub.Integration.Surf.Recurrence;
using DAO.Hub.UserDAO;
using Business.API.Hub.Entry;
using DTO.Hub.Cellphone.Enum;
using DTO.Hub.Order.Enum;
using DTO.General.Log.Database;
using DTO.General.Log.Enum;
using DTO.Hub.Product.Enum;
using DTO.Integration.Surf.Recurrence.Enum;

namespace Business.API.Hub.Integration.Surf.Base
{
    public class BlBaseSurf
    {
        private readonly BlEntry BlEntry;
        private readonly HubAllyDAO HubAllyDAO;
        private readonly HubUserDAO HubUserDAO;
        private readonly HubOrderDAO HubOrderDAO;
        private readonly LogHistoryDAO LogHistoryDAO;
        private readonly HubCategoryDAO HubCategoryDAO;
        private readonly BlSurfCustomer BlSurfCustomer;
        private readonly HubCustomerDAO HubCustomerDAO;
        private readonly BlSurfRecurrence BlSurfRecurrence;
        private readonly HubAccountPlanDAO HubAccountPlanDAO;
        private readonly SurfMobilePlanDAO SurfMobilePlanDAO;
        private readonly BlSurfSubscription BlSurfSubscription;
        private readonly HubPaymentOrderDAO HubPaymentOrderDAO;
        private readonly HubCellphoneManagementDAO HubCellphoneManagementDAO;
        public BlBaseSurf(XDataDatabaseSettings settings)
        {
            BlEntry = new(settings);
            HubUserDAO = new(settings);
            HubAllyDAO = new(settings);
            HubOrderDAO = new(settings);
            LogHistoryDAO = new(settings);
            HubCategoryDAO = new(settings);
            BlSurfCustomer = new(settings);
            HubCustomerDAO = new(settings);
            BlSurfRecurrence = new(settings);
            HubAccountPlanDAO = new(settings);
            SurfMobilePlanDAO = new(settings);
            HubPaymentOrderDAO = new(settings);
            BlSurfSubscription = new(settings);
            HubCellphoneManagementDAO = new(settings);
        }

        public HubCellphoneManagement Get(string id) => HubCellphoneManagementDAO.FindById(id);

        public BaseApiOutput Update(HubCellphoneManagement input)
        {
            if (string.IsNullOrEmpty(input?.Id))
                return new("Requisição mal formada!");

            if (string.IsNullOrEmpty(input.ChipSerial))
                return new("Serial do Cartão não informado!");

            if (input.Mode == HubCellphoneManagementTypeEnum.Unknown)
                return new("Modo não informado!");

            if (input.Mode == HubCellphoneManagementTypeEnum.Portability)
            {
                if (string.IsNullOrEmpty(input.Portability?.OperatorId))
                    return new("Operadora da Portabilidade não informada!");

                if (string.IsNullOrEmpty(input.Portability.Number))
                    return new("Número da Portabilidade não informada!");
            }

            HubCellphoneManagementDAO.Update(input);
            return new(true);
        }

        public async Task<HubCellphoneFinishOrderOutput> FinishOrder(HubCellphoneOrderCreation input)
        {
            var validation = BasicFinishOrderValidation(input);
            if (!validation.Success)
                return new(validation.Message);

            var categoriesId = input.ProductsOrder?.Select(x => x.CategoryId);
            if (!(categoriesId?.Any() ?? false))
                return new("Nenhum Id de Categoria informado!");

            var validCategories = HubCategoryDAO.Find(x => categoriesId.Contains(x.Id) && x.CategoryType == HubProductTypeEnum.Cellphone)?.Select(x => x.Id);
            var products = input.ProductsOrder.Where(x => validCategories.Contains(x.CategoryId)).ToList();
            if (!products.Any())
                return new("Nenhum produto telefônico encontrado!");

            // Perde referência de memória e não substitui os dados do obj original
            var transferInput = JsonConvert.DeserializeObject<HubCellphoneOrderCreation>(JsonConvert.SerializeObject(input));
            transferInput.ProductsOrder = products;

            return await GetChipActivationOutputAsync(input, transferInput).ConfigureAwait(false);
        }

        public async Task<HubProductOrderResultCellphoneData> ReprocessCellphoneManagement(string id, string allyId)
        {
            var management = HubCellphoneManagementDAO.FindById(id);
            if (management == null)
                return new("Gerenciamento Telefônico não encontrado!");

            if (management.AllyId != allyId)
            {
                HubOrderDAO.UpdateOrderStatus(management.OrderId, HubOrderStatusEnum.CellphoneError);
                return new("Gerenciamento Telefônico não pertence a este aliado!");
            }

            var surfCustomer = await BlSurfCustomer.GetSurfCustomer(management.CustomerId).ConfigureAwait(false);
            if (!(surfCustomer?.Success ?? false))
            {
                HubOrderDAO.UpdateOrderStatus(management.OrderId, HubOrderStatusEnum.CellphoneError);
                return new(surfCustomer?.Message ?? "Não foi possível encontrar o Cliente Telefonia.", management.ProductOrderId, management.SurfMobilePlanId);
            }

            var managementStatus = management.Status;
            if (managementStatus == HubCellphoneManagementStatusEnum.AwaitingPayment)
            {
                HubOrderDAO.UpdateOrderStatus(management.OrderId, HubOrderStatusEnum.AwaitingPayment);
                return new("Aguardando Pagamento.", management.ProductOrderId, management.SurfMobilePlanId);
            }

            if (managementStatus == HubCellphoneManagementStatusEnum.Create)
            {
                var stepTwo = BlSurfSubscription.SubscriptionStepTwo(new(management.Id, surfCustomer.Code));
                if (!(stepTwo?.Success ?? false))
                {
                    HubOrderDAO.UpdateOrderStatus(management.OrderId, HubOrderStatusEnum.CellphoneError);
                    return new(stepTwo?.Message ?? "Não foi possível adicionar o Cliente.", management.ProductOrderId, management.SurfMobilePlanId);
                }

                managementStatus = HubCellphoneManagementStatusEnum.AddSurfCustomer;
            }

            if (managementStatus == HubCellphoneManagementStatusEnum.AddSurfCustomer)
            {
                var stepThree = await BlSurfSubscription.SubscriptionStepThree(new(new(management), management.Id)).ConfigureAwait(false);
                if (!(stepThree?.Success ?? false))
                {
                    HubOrderDAO.UpdateOrderStatus(management.OrderId, HubOrderStatusEnum.CellphoneError);
                    return new(stepThree?.Message ?? "Não foi possível ativar o Chip.", management.ProductOrderId, management.SurfMobilePlanId);
                }

                managementStatus = HubCellphoneManagementStatusEnum.ChipActive;
            }

            if (managementStatus == HubCellphoneManagementStatusEnum.ChipActive)
            {
                if (management.Mode == HubCellphoneManagementTypeEnum.Portability)
                {
                    var stepFour = BlSurfSubscription.SubscriptionStepFour(new(new(management), management.Id));
                    if (!(stepFour?.Success ?? false))
                    {
                        HubOrderDAO.UpdateOrderStatus(management.OrderId, HubOrderStatusEnum.CellphoneError);
                        return new(stepFour?.Message ?? "Não foi possível realizar a Portabilidade.", management.ProductOrderId, management.SurfMobilePlanId);
                    }
                }

                var stepFive = BlSurfSubscription.SubscriptionStepFive(management.Id);
                if (!(stepFive?.Success ?? false))
                {
                    HubOrderDAO.UpdateOrderStatus(management.OrderId, HubOrderStatusEnum.CellphoneError);
                    return new(stepFive?.Message ?? "Não foi possível finalizar o Gerenciamento.", management.ProductOrderId, management.SurfMobilePlanId);
                }
            }

            HubOrderDAO.UpdateOrderStatus(management.OrderId, HubOrderStatusEnum.Finished);
            management = HubCellphoneManagementDAO.FindById(id);
            return new(management, management.ProductOrderId);
        }

        public HubProductOrderResultCellphoneData GetCellphoneData(string productOrderId)
        {
            var management = HubCellphoneManagementDAO.FindOne(x => x.ProductOrderId == productOrderId);
            if (management == null)
                return null;

            return new(management, management.ProductOrderId);
        }

        public async Task<HubAddSubscriptionOutput> AddSubscription(HubCellphoneManagementStepOneInput input)
        {
            var stepOne = BlSurfSubscription.SubscriptionStepOne(input);
            if (!(stepOne?.Success ?? false))
                return new(stepOne?.Message ?? "Não foi possível cadastrar o Gerenciamento.");

            var surfCustomer = await BlSurfCustomer.GetSurfCustomer(input.CustomerId).ConfigureAwait(false);
            if (!(surfCustomer?.Success ?? false))
                return new(surfCustomer?.Message ?? "Não foi possível encontrar o Cliente Telefonia.", stepOne.CellphoneManagementId);

            var stepTwo = BlSurfSubscription.SubscriptionStepTwo(new(stepOne.CellphoneManagementId, surfCustomer.Code));
            if (!(stepTwo?.Success ?? false))
                return new(stepTwo?.Message ?? "Não foi possível adicionar o Cliente ao Gerenciamento.", stepOne.CellphoneManagementId);

            var stepThree = await BlSurfSubscription.SubscriptionStepThree(new(input, stepOne.CellphoneManagementId)).ConfigureAwait(false);
            if (!(stepThree?.Success ?? false))
                return new(stepThree?.Message ?? "Não foi possível ativar o Chip.", stepOne.CellphoneManagementId);

            if (input.Mode == HubCellphoneManagementTypeEnum.Portability)
            {
                var stepFour = BlSurfSubscription.SubscriptionStepFour(new(input, stepOne.CellphoneManagementId));
                if (!(stepFour?.Success ?? false))
                    return new(stepFour?.Message ?? "Não foi possível realizar a Portabilidade.", stepOne.CellphoneManagementId);
            }

            var stepFive = BlSurfSubscription.SubscriptionStepFive(stepOne.CellphoneManagementId);
            if (!(stepFive?.Success ?? false))
                return new(stepFive?.Message ?? "Não foi possível finalizar o Gerenciamento.", stepOne.CellphoneManagementId);

            var result = new HubAddSubscriptionOutput(true);
            var cellphoneData = GetCellphoneById(stepOne.CellphoneManagementId);
            if (cellphoneData != null)
            {
                result.ICCID = cellphoneData.ICCID;
                result.Number = cellphoneData.Number;
            }

            return result;
        }

        public HubRecurrenceListOutput List(HubRecurrenceListInput input)
        {
            var result = HubCellphoneManagementDAO.List(input);
            if (!(result?.Any() ?? false))
                return new("Nenhum Gerenciamento de Telefone encontrado!");

            return new(result);
        }

        public HubCellphoneDetailsOutput GetCellphoneDetails(string managementId)
        {
            var management = HubCellphoneManagementDAO.FindById(managementId);
            if (management == null)
                return new("Gerenciamento Telefônico não encontrado!");

            var customer = HubCustomerDAO.FindById(management.CustomerId);
            if (customer == null)
                return new("Cliente não encontrado!");

            var surfMobilePlan = SurfMobilePlanDAO.FindById(management.SurfMobilePlanId);
            if (surfMobilePlan == null)
                return new("Plano de Telefone não encontrado!");

            return new(management, customer, surfMobilePlan);
        }

        public async Task<BaseApiOutput> ChangeRecurrenceStatus(HubCellphoneRecurrenceChangeInput input, SurfRecurrenceStatus type)
        {
            if (input == null)
                return new("Requisição mal formada!");

            var management = HubCellphoneManagementDAO.FindById(input.ManagementId);
            if (management == null)
                return new("Gerenciamento Telefônico não encontrado!");

            if (management.AllyId != input.AllyId)
                return new("Gerenciamento Telefônico não pertence a este aliado!");

            var user = HubUserDAO.FindById(input.UserId);
            if (user == null)
                return new("Usuário não encontrado!");

            var result = await BlSurfRecurrence.ChangeRecurrenceStatus(input.ManagementId, type).ConfigureAwait(false);
            if (result?.Success ?? false)
            {
                LogHistoryDAO.Insert(new AppLogHistory
                {
                    Message = $"Recorrência {management.Id} alterada pelo usuário {user.Name}!",
                    Type = AppLogTypeEnum.XApiInfo,
                    Method = "ChangeRecurrenceStatus",
                    Date = DateTime.Now
                });
            }

            return result;
        }

        public BaseApiOutput CellphonePaymentCofirmed(string orderId)
        {
            var payments = HubPaymentOrderDAO.FindByOrderId(orderId);
            if (!(payments?.Any() ?? false))
                return new("Pagamentos não encontrados!");

            if (payments.Any(x => x.Status != HubPaymentOrderType.Paid))
                return new("Ainda há pagamentos pendentes!");

            var cellphones = HubCellphoneManagementDAO.FindByOrderId(orderId);
            if (!(cellphones?.Any() ?? false))
                return new("Venda informada não possui telefones vinculados!");

            foreach (var cellphone in cellphones)
            {
                cellphone.Status = HubCellphoneManagementStatusEnum.AddSurfCustomer;
                HubCellphoneManagementDAO.Update(cellphone);
            }

            return new(true);
        }

        private HubProductOrderResultCellphoneData GetCellphoneById(string id)
        {
            var management = HubCellphoneManagementDAO.FindById(id);
            if (management == null)
                return null;

            return new(management, management.ProductOrderId);
        }

        private async Task<HubCellphoneFinishOrderOutput> GetChipActivationOutputAsync(HubCellphoneOrderCreation input, HubCellphoneOrderCreation transferInput)
        {
            if (input?.Order == null || transferInput?.Order == null)
                return new("Dados inválidos informados!");

            var productsWithErrors = new List<HubFinishCellphoneResult>();
            foreach (var product in transferInput.ProductsOrder)
            {
                var productWithErrors = new HubFinishCellphoneResult(product.ProductOrderId);

                var surfPlanId = SurfMobilePlanDAO.FindById(product.CellphoneData?.SurfMobilePlanId)?.Id;
                if (string.IsNullOrEmpty(surfPlanId))
                    productWithErrors.Message = $"{product.Name }: O plano da Surf não foi encontrado.";

                if (string.IsNullOrEmpty(product.ProductOrderId))
                    productWithErrors.Message = $"{product.Name }: O item da venda não foi encontrado.";

                if (string.IsNullOrEmpty(productWithErrors.Message))
                {
                    var resultSubscription = await AddSubscription(new(product.CellphoneData, transferInput.Order.AllyId, input.Order.OrderId, product.ProductOrderId, transferInput.Order.CustomerId, surfPlanId, product.OrderPrice)).ConfigureAwait(false);
                    if (!(resultSubscription?.Success ?? false))
                        productWithErrors.Message = $"{product.Name }: {resultSubscription?.Message ?? "Não foi possível ativar o chip."}";
                }

                productWithErrors.Success = string.IsNullOrEmpty(productWithErrors.Message);
                if (productWithErrors.Success)
                    continue;

                productsWithErrors.Add(productWithErrors);
            }

            if (productsWithErrors?.Select(x => x.Message).Any() ?? false)
                return new(string.Join(" ", productsWithErrors.Select(x => x.Message)), productsWithErrors);

            if (input.ProductsOrder.Count != transferInput.ProductsOrder.Count)
                return new($"Chips ativados com sucesso, porém os produtos { string.Join(", ", input.ProductsOrder.Except(transferInput.ProductsOrder).Select(x => x.Name)) }, não pertencem a categoria telefônica!", productsWithErrors);

            return new(true);
        }

        private BaseApiOutput BasicFinishOrderValidation(HubCellphoneOrderCreation input)
        {
            if (string.IsNullOrEmpty(input?.Order?.OrderId))
                return new("Id da venda não informado!");

            if (!(input.ProductsOrder?.Any(x => x.CellphoneData != null) ?? false))
                return new("Nenhum produto telefônico informado!");

            var masterAlly = HubAllyDAO.FindOne(x => x.IsMasterAlly);
            if (masterAlly == null)
                return new("Aliado master não encontrado!");

            var expenseAccountPlanName = HubAccountPlanDAO.FindById(masterAlly?.ExpenseAccountPlanId)?.Name;
            if (string.IsNullOrEmpty(expenseAccountPlanName))
                return new("Plano de Conta de repasse não encontrado!");

            return new(true);
        }
    }
}
