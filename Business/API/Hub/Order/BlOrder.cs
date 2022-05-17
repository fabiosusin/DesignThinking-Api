
using Business.API.Hub.BitDefender;
using Business.API.Hub.Entry;
using Business.API.Hub.Integration.Asaas.Customer;
using Business.API.Hub.Integration.Sige.Order;
using Business.API.Hub.Integration.Surf.Base;
using Business.API.Hub.NFSe;
using DAO.DBConnection;
using DAO.General.Log;
using DAO.Hub.AccountPlan;
using DAO.Hub.AllyDAO;
using DAO.Hub.Cellphone;
using DAO.Hub.Company;
using DAO.Hub.CustomerDAO;
using DAO.Hub.Order;
using DAO.Hub.Product;
using DAO.Hub.UserDAO;
using DTO.General.Base.Api.Output;
using DTO.General.Log.Database;
using DTO.General.Log.Enum;
using DTO.Hub.Ally.Enum;
using DTO.Hub.BitDefender.Input;
using DTO.Hub.Cellphone.Enum;
using DTO.Hub.Cellphone.Input;
using DTO.Hub.Cellphone.Output;
using DTO.Hub.NFSe.Output;
using DTO.Hub.Order.Database;
using DTO.Hub.Order.Enum;
using DTO.Hub.Order.Input;
using DTO.Hub.Order.Output;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Utils.Extensions.Files.Images.ImageFactory;

namespace Business.API.Hub.Order
{
    public class BlOrder
    {
        private readonly BlNfse BlNfse;
        private readonly BlEntry BlEntry;
        private readonly BlLicense BlLicense;
        private readonly HubUserDAO HubUserDAO;
        private readonly BlBaseSurf BlBaseSurf;
        private readonly HubAllyDAO HubAllyDAO;
        private readonly HubOrderDAO HubOrderDAO;
        private readonly BlSigeOrder BlSigeOrder;
        private readonly HubCompanyDAO HubCompanyDAO;
        private readonly LogHistoryDAO LogHistoryDAO;
        private readonly HubProductDAO HubProductDAO;
        private readonly BlAsaasCharge BlAsaasCharge;
        private readonly HubCustomerDAO HubCustomerDAO;
        private readonly BlPaymentOrder BlPaymentOrder;
        private readonly HubAccountPlanDAO HubAccountPlanDAO;
        private readonly HubProductOrderDAO HubProductOrderDAO;
        private readonly HubPaymentOrderDAO HubPaymentOrderDAO;
        private readonly HubCellphoneManagementDAO HubCellphoneManagementDAO;

        public BlOrder(XDataDatabaseSettings settings)
        {
            BlNfse = new(settings);
            BlEntry = new(settings);
            BlLicense = new(settings);
            HubUserDAO = new(settings);
            HubAllyDAO = new(settings);
            BlBaseSurf = new(settings);
            HubOrderDAO = new(settings);
            BlSigeOrder = new(settings);
            BlAsaasCharge = new(settings);
            LogHistoryDAO = new(settings);
            HubCompanyDAO = new(settings);
            HubProductDAO = new(settings);
            HubCustomerDAO = new(settings);
            BlPaymentOrder = new(settings);
            HubAccountPlanDAO = new(settings);
            HubPaymentOrderDAO = new(settings);
            HubProductOrderDAO = new(settings);
            HubCellphoneManagementDAO = new(settings);
        }

        public async Task<HubCreateOrderOutput> CreateOrder(HubOrderCreationInput input)
        {
            HubCreateOrderOutput result = null;
            string orderId = null;

            try
            {
                #region Validação e Ajustes dos Dados 
                var basicValidation = BasicValidation(input);
                if (!basicValidation.Success)
                    return new(basicValidation.Message);

                var company = string.IsNullOrEmpty(input.Order.CompanyId) ? HubCompanyDAO.GetDefaultCompany() : HubCompanyDAO.FindOne(x => x.Id == input.Order.CompanyId && x.DefaultDepositId == input.Order.DepositId);
                if (company == null)
                    return new("Empresa não encontrada!");

                var accountPlan = !string.IsNullOrEmpty(input.Order.AccountPlanId) ? HubAccountPlanDAO.FindById(input.Order.AccountPlanId) : HubAccountPlanDAO.GetAllyAccountPlan(input.Order.Ally.Id);
                if (string.IsNullOrEmpty(accountPlan?.Name))
                    return new("Plano de Contas não informado!");

                var orderPrice = GetOrderPrice(input.ProductsOrder);
                if ((input.Order.Payments?.Sum(x => x.Value) ?? 0) != orderPrice.Price)
                    return new("Os valores informados no Pagamento não batem com o valor total da Venda!");

                input.Order.AccountPlanId = accountPlan.Id;
                input.Order.CompanyId = company.Id;
                input.Order.DepositId = company.DefaultDepositId;
                #endregion

                #region Geração da Venda no HUB
                var orderResultInsert = HubOrderDAO.Insert(new(input.Order, orderPrice, SaveImageFromBase64(input.Order.Customer.DocumentBase64, 500, GetImagesEnum.Jpeg)?.ImageJpeg));
                if (!orderResultInsert.Success)
                    return new(orderResultInsert.Message);

                var orderResult = (HubOrder)orderResultInsert.Data;
                orderId = orderResult.Id;
                result = new HubCreateOrderOutput(true, orderId);
                #endregion

                #region Cadastro de Produtos no HUB
                var registerProductsResult = RegisterProducts(new(input.ProductsOrder, orderId));
                if (!registerProductsResult.Success)
                {
                    CleanOrderData(orderId);
                    return new(registerProductsResult.Message);
                }
                #endregion

                #region Geração da Cobrança
                _ = BlEntry.CreateOrderEntry(orderId, HubOrderEntryTypeEnum.AllyShare);

                var ally = HubAllyDAO.FindById(orderResult.AllyId);
                if ((input.Order.Payments?.Any() ?? false) && ally?.ChargeType == HubAllyChargeTypeEnum.Integrated)
                {
                    var asaasCharges = await BlAsaasCharge.CreateCharges(orderId, input.Order.Payments).ConfigureAwait(false);
                    if (asaasCharges == null)
                    {
                        CleanOrderData(orderId);
                        return new("Não foi possível realizar as cobranças");
                    }

                    var chargeInserResult = BlPaymentOrder.SaveOrderAsaasCharges(orderId, asaasCharges);
                    if (!chargeInserResult.Success)
                        return new(chargeInserResult.Message);

                    result.Order.Payments = await BlPaymentOrder.GetChargesList(orderId).ConfigureAwait(false);
                }
                #endregion

                #region Ativação de Chip
                var cellphoneProducts = input.ProductsOrder.Where(x => x.CellphoneData != null);
                if (cellphoneProducts?.Any() ?? false)
                {
                    _ = BlEntry.CreateOrderEntry(orderId, HubOrderEntryTypeEnum.SurfCost);

                    var cellphoneResult = await RegisterCellphoneNumbers(orderId).ConfigureAwait(false);
                    if (cellphoneResult?.Status != HubCellphoneManagementStatusEnum.Completed)
                    {
                        orderResult.Status = cellphoneResult?.Status == HubCellphoneManagementStatusEnum.AwaitingPayment ? HubOrderStatusEnum.AwaitingPayment : HubOrderStatusEnum.CellphoneError;
                        HubOrderDAO.UpdateStatus(orderResult.Id, orderResult.Status);
                    }
                }
                #endregion

                #region Atualizar Licenças BitDefender
                var bitDefenderCategories = input.ProductsOrder.Where(x => !string.IsNullOrEmpty(x.BitDefenderCategoryId));
                if (bitDefenderCategories?.Any() ?? false)
                {
                    var resultLicenses = BlLicense.MarkLicensesAsUsed(bitDefenderCategories.Select(x => new BitDefenderUseLicensesInput(x.BitDefenderCategoryId, (int)x.Quantity)), orderId, orderResult.AllyId);
                    if (!resultLicenses.Success)
                    {
                        CleanOrderData(orderId);
                        return new(registerProductsResult.Message);
                    }
                }
                #endregion

                #region Geração da Venda no ERP
                var sigeOrderCode = await RegisterSigeOrder(new(input, company, accountPlan)).ConfigureAwait(false);
                if (sigeOrderCode > 0)
                    HubOrderDAO.UpdateSigeCode(orderResult.Id, sigeOrderCode);
                #endregion

                #region Finalização da Venda
                _ = await GenerateOrderNfse(orderId).ConfigureAwait(false);

                if (orderResult.Status == HubOrderStatusEnum.Created)
                {
                    orderResult.Status = HubOrderStatusEnum.Finished;
                    HubOrderDAO.UpdateStatus(orderResult.Id, HubOrderStatusEnum.Finished);
                }
                #endregion

                result.Products = GetProductsDetails(orderId);
            }
            catch (Exception e)
            {
                LogHistoryDAO.Insert(new AppLogHistory
                {
                    Message = "Erro ao gerar venda no HUB!",
                    ExceptionMessage = e.Message,
                    Type = AppLogTypeEnum.XApiExceptionError,
                    Method = "CreateOrder",
                    Date = DateTime.Now
                });

                CleanOrderData(orderId);
            }

            return result;
        }

        public async Task<HubOrderCreationChargeOutput> ReprocessOrderCharge(string paymentOrderId, HubOrderInputPaymentData payment)
        {
            var paymentOrder = HubPaymentOrderDAO.FindById(paymentOrderId);
            if (paymentOrder == null)
                return new(new("Pagamento não encontado!"));

            var result = await BlAsaasCharge.CreateCharge(paymentOrder.OrderId, payment).ConfigureAwait(false);
            if (result.Error == null)
            {
                paymentOrder.AsaasData = BlAsaasCharge.GetAsaasData(result);
                HubPaymentOrderDAO.Update(paymentOrder);
            }

            result.PaymentOrderId = paymentOrderId;
            return result;
        }

        public async Task<HubOrderDetailsOutput> GetOrderDetails(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
                return new("Requisição mal formada!");

            var order = HubOrderDAO.FindById(orderId);
            if (order == null)
                return new("Venda não encontrada!");

            var result = new HubOrderDetailsOutput(true, new(order, HubCustomerDAO.FindById(order.Customer?.CustomerId)), GetProductsDetails(orderId));
            result.Order.Payments = await BlPaymentOrder.GetChargesList(order.Id).ConfigureAwait(false);

            return result;
        }

        public async Task<BaseApiOutput> GenerateOrderNfse(string orderId)
        {
            try
            {
                var nfseOutput = await BlNfse.GenerateCustomerNfse(orderId);
                if (nfseOutput.Success)
                {
                    HubOrderDAO.UpdateNFSeData(orderId, new(nfseOutput.NfseOutput?.Lot));
                    _ = await GetOrderNfseStatus(orderId).ConfigureAwait(false);
                }
                else
                    LogHistoryDAO.Insert(new AppLogHistory
                    {
                        Message = "Erro ao gerar NFSe!",
                        ExceptionMessage = nfseOutput.Message,
                        Type = AppLogTypeEnum.XApiNfseRequestError,
                        Method = "GenerateOrderNfse",
                        Date = DateTime.Now
                    });

                return nfseOutput;
            }
            catch (Exception e)
            {
                LogHistoryDAO.Insert(new AppLogHistory
                {
                    Message = "Erro desconhecido ao gerar NFSe!",
                    ExceptionMessage = e.Message,
                    Type = AppLogTypeEnum.XApiNfseRequestError,
                    Method = "GenerateOrderNfse",
                    Date = DateTime.Now
                });

                return new("Ocorreu um erro ao emitir a NFSe");
            }
        }

        public async Task<HubNfseStatusOutput> GetOrderNfseStatus(string orderId)
        {
            var result = await BlNfse.GetNfseStatus(orderId);
            if (result.Success)
            {
                var order = HubOrderDAO.FindById(orderId);
                if (order != null)
                    HubOrderDAO.UpdateNFSeData(orderId, new(order.Nfse.Lot, result.NfseOutput?.Nfse?.AccessKey));
            }
            else
            {
                LogHistoryDAO.Insert(new AppLogHistory
                {
                    Message = "Erro ao buscar status da NFSe!",
                    ExceptionMessage = result.Message + $"OrderId: {orderId}",
                    Type = AppLogTypeEnum.XApiExceptionError,
                    Method = "GetOrderNfseStatus",
                    Date = DateTime.Now
                });
            }

            return result;
        }

        public async Task<HubNfseDetailsOutput> GetOrderNfseDetails(string orderId) => await BlNfse.GetNfseDetails(orderId);

        public HubOrderListOutput List(HubOrderListInput input)
        {
            var result = HubOrderDAO.List(input);
            if (!(result?.Any() ?? false))
                return new("Nenhuma Venda encontrada!");

            return new(result);
        }

        private List<HubProductOrderDetailsOutput> GetProductsDetails(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
                return null;

            var order = HubOrderDAO.FindById(orderId);
            if (order == null)
                return null;

            var productsOrder = HubProductOrderDAO.GetProductsOrder(order.Id);
            var productOrderIds = productsOrder?.Select(x => x.ProductId);
            if (!(productOrderIds?.Any() ?? false))
                return null;

            var products = HubProductDAO.GetProductsOrderDetails(productOrderIds);
            var result = new List<HubProductOrderDetailsOutput>();
            foreach (var productOrder in productsOrder)
            {
                var product = products.FirstOrDefault(y => y.Id == productOrder.ProductId);
                var data = new HubProductOrderDetailsOutput(product, productOrder.Id);
                if (data == null)
                    continue;

                data.Price = productOrder.Price;
                if (!string.IsNullOrEmpty(product.BitDefenderCategoryId))
                    data.BitDefenderData = BlLicense.GetUseLicensesOutput(orderId, product.BitDefenderCategoryId)?.Data;

                if (!string.IsNullOrEmpty(product.SurfMobilePlanId))
                {
                    data.CellphoneData = BlBaseSurf.GetCellphoneData(productOrder.Id);
                    if (data.CellphoneData == null)
                        data.CellphoneData = new("Não foi possível salvar os dados do Telefone!", productOrder.Id, product.SurfMobilePlanId);
                }

                result.Add(data);
            }

            return result;
        }

        private static OrderPrice GetOrderPrice(List<HubProductOrderInput> products)
        {
            if (!(products?.Any() ?? false))
                return null;

            var result = new OrderPrice();
            foreach (var product in products)
            {
                result.Cost += (product.Price?.Cost ?? 0) * product.Quantity;
                result.XPlayShare += (product.Price?.XPlayShare ?? 0) * product.Quantity;
                result.AllyShare += (product.Price?.AllyShare ?? 0) * product.Quantity;
                result.Tax += (product.Price?.Tax ?? 0) * product.Quantity;
                result.Price += (product.Price?.Price ?? 0) * product.Quantity;
                result.ChipPrice = (product.Price?.ChipPrice ?? 0) * product.Quantity;
            }

            return result;
        }

        private BaseApiOutput RegisterProducts(HubOrderRegisterProductsInput input)
        {
            if (!(input?.Products?.Any() ?? false))
                return new("Nenhum produto informado");

            if (input.Products?.Any(x => string.IsNullOrEmpty(x.ProductId)) ?? false)
                return new("Todos Ids de Produto devem ser informados");

            if (string.IsNullOrEmpty(input.OrderId))
                return new("Id da venda não informado");

            var cellphonesProducts = HubProductDAO.Find(x => !string.IsNullOrEmpty(x.SurfMobilePlanId));
            foreach (var product in input.Products)
            {
                if (product.CellphoneData != null)
                {
                    var existingProduct = cellphonesProducts?.FirstOrDefault(x => x.Id == product.ProductId && x.SurfMobilePlanId == product.CellphoneData.SurfMobilePlanId);
                    if (existingProduct == null)
                        return new($"O Produto {product.Name} não possui o plano celular informado!");
                }

                var resultInsert = HubProductOrderDAO.Insert(new(input.OrderId, product.ProductId, product));
                if (!resultInsert.Success)
                    return new(resultInsert.Message);
            }

            return new(true);
        }

        private async Task<HubOrderRegisterCellphoneNumbersOutput> RegisterCellphoneNumbers(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
                return new("Id de Venda não informado");

            var order = HubOrderDAO.FindById(orderId);
            if (order == null)
                return new("Venda não encontrada");

            var orderProducts = HubProductOrderDAO.GetProductsOrder(orderId)?.Where(x => x.CellphoneData != null);
            if (!(orderProducts?.Any() ?? false))
                return new("Venda não possui venda de linhas celulares");

            var productsId = orderProducts.Select(x => x.ProductId).ToList();
            var mobileProducts = HubProductDAO.Find(x => productsId.Contains(x.Id));

            var surfResult = await BlBaseSurf.FinishOrder(new(order, orderProducts.Select(x => new HubProductCellphoneInput(x, mobileProducts?.FirstOrDefault(y => y.Id == x.ProductId)?.Name)))).ConfigureAwait(false);
            if (surfResult?.Success ?? false)
                return new(HubCellphoneManagementStatusEnum.Completed, GetCellphoneProducts(surfResult, orderProducts));

            var status = HubCellphoneManagementStatusEnum.Unknown;
            var managementAwaitingPayment = HubCellphoneManagementDAO.FindByOrderId(orderId)?.Where(x => x.Status != HubCellphoneManagementStatusEnum.Completed);
            if (managementAwaitingPayment?.Any() ?? false)
            {
                status = managementAwaitingPayment.FirstOrDefault(x => x.Status == HubCellphoneManagementStatusEnum.AwaitingPayment) != null ?
                    HubCellphoneManagementStatusEnum.AwaitingPayment : managementAwaitingPayment.FirstOrDefault().Status;
            }

            return new(status, surfResult?.Message ?? "Não foi possível finalizar o registro Telefônico", GetCellphoneProducts(surfResult, orderProducts));
        }

        private List<HubProductOrderResultCellphoneData> GetCellphoneProducts(HubCellphoneFinishOrderOutput surfResult, IEnumerable<HubProductOrder> orderProducts)
        {
            if (!(orderProducts?.Any() ?? false))
                return null;

            var cellphonesData = new List<HubProductOrderResultCellphoneData>();
            foreach (var cellphoneProduct in orderProducts)
            {
                var errorProductMessage = surfResult?.CellphonesWithErrors?.FirstOrDefault(x => x.ProductOrderId == cellphoneProduct.Id)?.Message;

                var cellphoneDataResult =
                    BlBaseSurf.GetCellphoneData(cellphoneProduct.Id) ??
                    new(errorProductMessage ?? "Não foi possível salvar os dados do Telefone!", cellphoneProduct.Id, cellphoneProduct.SurfMobilePlanId);

                if (!string.IsNullOrEmpty(errorProductMessage))
                    cellphoneDataResult.ErrorMessage = errorProductMessage;

                cellphonesData.Add(cellphoneDataResult);
            }

            return cellphonesData;
        }

        private async Task<long> RegisterSigeOrder(HubOrderRegisterSigeOrderInput input)
        {
            var ally = HubAllyDAO.FindById(input.InputOrderCreation?.Order?.Ally?.Id);
            if (ally == null)
                return 0L;

            if (string.IsNullOrEmpty(input.InputOrderCreation.Order.Ally.Cnpj))
                input.InputOrderCreation.Order.Ally.Cnpj = ally.Cnpj;

            var resultSigeOrder = await BlSigeOrder.CreateSigeOrder(input.InputOrderCreation.Order, input.Company, input.InputOrderCreation.ProductsOrder, input.AccountPlan?.Name).ConfigureAwait(false);
            if (!(resultSigeOrder?.Success ?? false))
            {
                LogHistoryDAO.Insert(new AppLogHistory
                {
                    Message = "Erro ao criar venda no ERP!",
                    ExceptionMessage = resultSigeOrder?.Mensagem ?? "Não foi possível gerar uma venda no ERP para o Aliado " + ally.Name,
                    Type = AppLogTypeEnum.XApiSigeRequestError,
                    Method = "RegisterSigeOrderAsync",
                    Data = JsonConvert.SerializeObject(input),
                    Date = DateTime.Now
                });
            }

            return resultSigeOrder?.Pedido?.Codigo ?? 0;
        }

        private void CleanOrderData(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
                return;

            HubProductOrderDAO.RemoveProductsOrder(orderId);
            HubOrderDAO.RemoveById(orderId);
        }

        private BaseApiOutput BasicValidation(HubOrderCreationInput input)
        {
            if (input == null)
                return new("Requisição mal formada!");

            if (input.Order == null)
                return new("Venda não informada!");

            if (string.IsNullOrEmpty(input.Order.Ally?.Id))
                return new("Id de Aliado não informado!");

            if (!(input.ProductsOrder?.Any() ?? false))
                return new("Produtos da venda não informados!");

            if (input.ProductsOrder?.Any(x => string.IsNullOrEmpty(x.ProductId)) ?? false)
                return new("Todos Ids de Produto devem ser informados!");

            if (input.ProductsOrder.Any(x => string.IsNullOrEmpty(x.CategoryId)))
                return new("Informe a categoria de todos os produtos da venda!");

            if (input.ProductsOrder.Any(x => string.IsNullOrEmpty(x.Name)))
                return new("Informe o nome de todos os produtos da venda!");

            if (HubCustomerDAO.FindById(input.Order.Customer?.Id)?.AllyId != input.Order.Ally.Id)
                return new("Cliente não pertence a esse Aliado!");

            if (HubUserDAO.FindById(input.Order.SellerId)?.AllyId != input.Order.Ally.Id)
                return new("Usuário não pertence a esse Aliado!");

            var orderCellphoneProducts = input.ProductsOrder.Where(x => x.CellphoneData != null).ToList();
            if (orderCellphoneProducts?.Any() ?? false)
            {
                var cellphonesProducts = HubProductDAO.Find(x => !string.IsNullOrEmpty(x.SurfMobilePlanId));
                foreach (var cellphoneProduct in orderCellphoneProducts)
                {
                    var product = cellphonesProducts?.FirstOrDefault(x => x.Id == cellphoneProduct.ProductId && x.SurfMobilePlanId == cellphoneProduct.CellphoneData.SurfMobilePlanId);
                    if (product == null)
                        return new($"O Produto {cellphoneProduct.Name} não possui o plano celular informado!");

                    if (cellphoneProduct.CellphoneData.Mode == HubCellphoneManagementTypeEnum.Unknown)
                        return new("Informe o Modo de Operação do Celular para todos os produtos telefônicos!");

                    if (cellphoneProduct.CellphoneData.Mode == HubCellphoneManagementTypeEnum.New && string.IsNullOrEmpty(cellphoneProduct.CellphoneData.DDD))
                        return new("Informe o DDD para o produto " + cellphoneProduct.Name);

                    if (cellphoneProduct.CellphoneData.Mode == HubCellphoneManagementTypeEnum.Portability)
                    {
                        if (string.IsNullOrEmpty(cellphoneProduct.CellphoneData.Portability?.Number))
                            return new("Informe o número da portabilidade para o produto " + cellphoneProduct.Name);

                        if (string.IsNullOrEmpty(cellphoneProduct.CellphoneData.Portability?.OperatorId))
                            return new("Informe a operadora da portabilidade para o produto " + cellphoneProduct.Name);
                    }

                    if (string.IsNullOrEmpty(cellphoneProduct.CellphoneData.ChipSerial))
                        return new("Informe o serial do chip para o produto " + cellphoneProduct.Name);
                }
            }

            return new(true);
        }
    }
}
