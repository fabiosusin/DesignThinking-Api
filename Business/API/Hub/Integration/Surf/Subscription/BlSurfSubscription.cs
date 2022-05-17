using DAO.DBConnection;
using DAO.General.Log;
using DAO.General.Surf;
using DAO.Mobile.Surf;
using DAO.Hub.Cellphone;
using DAO.Hub.CustomerDAO;
using DTO.General.Base.Api.Output;
using DTO.Integration.Surf.Subscription.Output;
using DTO.Hub.Cellphone.Database;
using DTO.Hub.Cellphone.Input;
using DTO.Hub.Cellphone.Output;
using Newtonsoft.Json;
using Services.Integration.Surf.Register.Portability;
using Services.Integration.Surf.Register.Subscription;
using System;
using System.Linq;
using System.Threading.Tasks;
using Useful.Extensions;
using Business.API.Hub.Integration.Surf.Portability;
using DAO.Hub.AllyDAO;
using DAO.Hub.Order;
using DTO.Hub.Order.Output;
using Business.API.Hub.Order;
using DTO.Hub.Cellphone.Enum;
using DTO.Hub.Integration.Asaas.Enum;
using DTO.Hub.Ally.Enum;
using DTO.General.Log.Enum;
using DTO.General.Log.Database;

namespace Business.API.Hub.Integration.Surf.Subscription
{
    public class BlSurfSubscription
    {
        protected SurfSubscriptionService SurfSubscriptionService;
        protected SurfPortabilityService SurfPortabilityService;
        protected HubCellphoneManagementDAO HubCellphoneManagementDAO;
        protected HubCustomerDAO HubCustomerDAO;
        protected SurfCustomerMsisdnDAO SurfCustomerMsisdnDAO;
        protected SurfMobilePlanDAO SurfMobilePlanDAO;
        protected LogHistoryDAO LogHistoryDAO;
        protected HubAllyDAO HubAllyDAO;
        protected HubOrderDAO HubOrderDAO;
        protected BlSurfPortability BlPortability;
        protected BlPaymentOrder BlPaymentOrder;

        public BlSurfSubscription(XDataDatabaseSettings settings)
        {
            SurfCustomerMsisdnDAO = new(settings);
            HubCustomerDAO = new(settings);
            HubCellphoneManagementDAO = new(settings);
            SurfMobilePlanDAO = new(settings);
            LogHistoryDAO = new(settings);
            HubAllyDAO = new(settings);
            HubOrderDAO = new(settings);
            BlPortability = new(settings);
            BlPaymentOrder = new(settings);
            SurfPortabilityService = new();
            SurfSubscriptionService = new();
        }

        // CellphoneManagementStatusEnum.Create
        public HubCellphoneManagementOutput SubscriptionStepOne(HubCellphoneManagementStepOneInput input)
        {
            var baseValidation = BasicValidationStepOne(input);
            if (!baseValidation.Success)
                return baseValidation;

            var surfPlan = SurfMobilePlanDAO.FindById(input.SurfMobilePlanId);
            if (surfPlan == null)
                return new("Plano não encontrado!");

            var result = HubCellphoneManagementDAO.Insert(new HubCellphoneManagement(input, surfPlan.SurfData));
            if (string.IsNullOrEmpty(result.Data?.Id))
                return new("Não foi possível salvar os dados de gerenciamento telefônico!");

            return new(true, result.Data.Id);
        }

        // CellphoneManagementStatusEnum.AddSurfCustomer
        public BaseApiOutput SubscriptionStepTwo(HubCellphoneManagementStepTwoInput input)
        {
            if (input == null)
                return new("Requisição mal formada!");

            if (string.IsNullOrEmpty(input.CellphoneManagementId))
                return new("Gerenciamento Telefônico não informado!");

            if (string.IsNullOrEmpty(input.SurfCustomerCode))
                return new("Cliente Telefonia não informado!");

            var management = HubCellphoneManagementDAO.FindById(input.CellphoneManagementId);
            if (management == null)
                return new("Gerenciamento Telefônico não encontrado!");

            if (management.Status != HubCellphoneManagementStatusEnum.Create)
                return new("Cliente já inserido no Gerenciamento!");

            management.SurfCustomerId = input.SurfCustomerCode;
            management.Status = HubCellphoneManagementStatusEnum.AddSurfCustomer;
            HubCellphoneManagementDAO.Update(management);

            return new(true);
        }

        // CellphoneManagementStatusEnum.ChipActive
        public async Task<BaseApiOutput> SubscriptionStepThree(HubCellphoneManagementStepThreeInput input)
        {
            if (input == null)
                return new("Requisição mal formada!");

            if (string.IsNullOrEmpty(input.CustomerId))
                return new("Cliente não informado!");

            if (string.IsNullOrEmpty(input.CellphoneManagementId))
                return new("Gerenciamento Telefônico não informado!");

            var management = HubCellphoneManagementDAO.FindById(input.CellphoneManagementId);
            if (management == null)
                return new("Gerenciamento Telefônico não encontrado!");

            if (management.Status != HubCellphoneManagementStatusEnum.AddSurfCustomer)
                return new("Chip já ativado para este Gerenciamento!");

            if (string.IsNullOrEmpty(management.CustomerId))
                return new("Cliente não informado!");

            if (!await ChipActivationEnabled(management.OrderId).ConfigureAwait(false))
            {
                management.Status = HubCellphoneManagementStatusEnum.AwaitingPayment;
                HubCellphoneManagementDAO.Update(management);
                return new("Chip será ativado após o pagamento!");
            }

            var payload = await GeneratePayload(management, input.CustomerId, management.SurfCustomerId).ConfigureAwait(false);
            if (!(payload?.Success ?? false))
                return new("Não foi possível ativar o chip!");

            management.SurfTransactionId = payload.Id;
            management.CellphoneData = GetPayloadCellphoneData(payload.Msisdn);
            management.Status = HubCellphoneManagementStatusEnum.ChipActive;
            HubCellphoneManagementDAO.Update(management);

            return new(true);
        }

        // CellphoneManagementStatusEnum.PortabilityActive
        public BaseApiOutput SubscriptionStepFour(HubCellphoneManagementStepFourInput input)
        {
            if (input == null)
                return new("Requisição mal formada!");

            if (input.Mode == HubCellphoneManagementTypeEnum.Unknown)
                return new("Modo não infomado!");

            if (input.Mode != HubCellphoneManagementTypeEnum.Portability)
                return new("Gerenciamento não é uma portabilidade!");

            if (string.IsNullOrEmpty(input.Portability?.OperatorId))
                return new("Operadora não infomada!");

            if (string.IsNullOrEmpty(input.Portability?.Number))
                return new("Número da portabilidade não infomado!");

            var management = HubCellphoneManagementDAO.FindById(input.CellphoneManagementId);
            if (management == null)
                return new("Gerenciamento Telefônico não encontrado!");

            if (management.Status != HubCellphoneManagementStatusEnum.ChipActive)
                return new("Portabilidade já criada para este Gerenciamento!");

            if (string.IsNullOrEmpty(management.CustomerId))
                return new("Cliente não informado!");

            var portability = BlPortability.GeneratePortability(management, input.Portability, management.CustomerId, management.SurfTransactionId);
            if (portability == null)
                return new("Não foi possível realizar a portabilidade!");

            input.Portability.Number = input.Portability.Number.GetMsisdn();
            management.Portability = input.Portability;
            management.CellphoneData = GetPayloadCellphoneData(management.Portability.Number);
            management.Status = HubCellphoneManagementStatusEnum.PortabilityActive;
            HubCellphoneManagementDAO.Update(management);

            return new(true);
        }

        // CellphoneManagementStatusEnum.Completed
        public BaseApiOutput SubscriptionStepFive(string cellphoneManagementId)
        {
            if (string.IsNullOrEmpty(cellphoneManagementId))
                return new("Requisição mal formada!");

            var management = HubCellphoneManagementDAO.FindById(cellphoneManagementId);
            if (management == null)
                return new("Gerenciamento Telefônico não encontrado!");

            if (management.Status == HubCellphoneManagementStatusEnum.Canceled)
                return new("Este Gerenciamento encontra-se cancelado!");

            var result = SurfCustomerMsisdnDAO.Insert(new(new(management)));
            if (!result.Success)
                return new(result.Message);

            management.RecurrenceDate = DateTime.Now.AddDays(30);
            management.Status = HubCellphoneManagementStatusEnum.Completed;
            HubCellphoneManagementDAO.Update(management);

            return new(true);
        }

        private async Task<SurfSubscriptionContentOutput> GeneratePayload(HubCellphoneManagement management, string customerId, string surfCustomerCode)
        {
            if (string.IsNullOrEmpty(customerId) || string.IsNullOrEmpty(surfCustomerCode))
                return null;

            var customer = HubCustomerDAO.FindById(customerId);
            if (customer == null)
                return null;

            try
            {
                var payload = (await SurfSubscriptionService.AddSubscription(new(surfCustomerCode, new(customer, management))).ConfigureAwait(false))?.Payload?.FirstOrDefault();
                return !(payload?.Success ?? false) ? null : payload;
            }
            catch (Exception e)
            {
                LogHistoryDAO.Insert(new AppLogHistory
                {
                    Message = "Erro ativar chip na Surf!",
                    Type = AppLogTypeEnum.XApiSurfRequestError,
                    Method = "GeneratePayload",
                    ExceptionMessage = e.Message,
                    Data = JsonConvert.SerializeObject(management),
                    Date = DateTime.Now
                });
                return null;
            }
        }

        private static HubCellphoneManagementOutput BasicValidationStepOne(HubCellphoneManagementStepOneInput input)
        {
            if (input == null)
                return new("Requisição mal formada!");

            if (string.IsNullOrEmpty(input.CustomerId))
                return new("Cliente não informado!");

            if (string.IsNullOrEmpty(input.OrderId))
                return new("Venda não informada!");

            if (string.IsNullOrEmpty(input.ProductOrderId))
                return new("Produto da Venda não informado!");

            if (string.IsNullOrEmpty(input.ChipSerial))
                return new("Serial do Chip não informado!");

            if (string.IsNullOrEmpty(input.SurfMobilePlanId))
                return new("Plano não informado!");

            if (string.IsNullOrEmpty(input.DDD))
                return new("DDD não informado!");

            if (!int.TryParse(input.DDD, out _))
                return new("DDD informado não estava num formato válido!");

            if (input.Mode == HubCellphoneManagementTypeEnum.Unknown)
                return new("Modo não infomado!");

            if (input.Mode == HubCellphoneManagementTypeEnum.Portability)
            {
                if (string.IsNullOrEmpty(input.Portability.OperatorId))
                    return new("Operadora não infomada!");

                if (string.IsNullOrEmpty(input.Portability.Number))
                    return new("Número da portabilidade não infomado!");
            }

            return new(true);
        }

        private static SurfCellphoneData GetPayloadCellphoneData(string msisdn) => string.IsNullOrEmpty(msisdn) ? null : new(msisdn);

        private async Task<bool> ChipActivationEnabled(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
                return false;

            var order = HubOrderDAO.FindById(orderId);
            if (order == null)
                return false;

            var ally = HubAllyDAO.FindById(order.AllyId);
            if (ally == null)
                return false;

            if (ally.ChargeType == HubAllyChargeTypeEnum.Independent)
                return true;

            var charges = await BlPaymentOrder.GetChargesList(order.Id).ConfigureAwait(false);
            if (charges == null)
                return false;

            foreach (var charge in charges)
            {
                if (charge.PaymentForm == HubOrderPaymentFormsOutput.Money)
                    continue;

                var paid = charge.Status == HubAsaasPaymentStatusEnum.Confirmed ||
                        charge.Status == HubAsaasPaymentStatusEnum.ReceivedInCash ||
                        charge.Status == HubAsaasPaymentStatusEnum.Received ||
                        charge.Status == HubAsaasPaymentStatusEnum.DunningReceived;

                if (!paid)
                    return false;
            }

            return true;
        }
    }
}
