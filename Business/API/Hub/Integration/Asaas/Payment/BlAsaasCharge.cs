using DAO.DBConnection;
using DAO.General.Log;
using DAO.Hub.AllyDAO;
using DAO.Hub.CustomerDAO;
using DAO.Hub.Order;
using DTO.General.Log.Database;
using DTO.General.Log.Enum;
using DTO.Hub.Ally.Enum;
using DTO.Hub.Integration.Asaas.Enum;
using DTO.Hub.Order.Database;
using DTO.Hub.Order.Enum;
using DTO.Hub.Order.Input;
using DTO.Hub.Order.Output;
using DTO.Integration.Asaas.Base.Output;
using DTO.Integration.Asaas.Payments.Input;
using DTO.Integration.Asaas.Payments.Output;
using Services.Integration.Asaas.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.API.Hub.Integration.Asaas.Customer
{
    public class BlAsaasCharge
    {
        protected AsaasChargeService AsaasChargeService;
        protected LogHistoryDAO LogHistoryDAO;
        protected HubOrderDAO HubOrderDAO;
        protected HubCustomerDAO HubCustomerDAO;
        protected HubAllyDAO HubAllyDAO;
        protected BlAsaasCustomer BlAsaasCustomer;
        protected HubPaymentOrderDAO HubPaymentOrderDAO;
        public BlAsaasCharge(XDataDatabaseSettings settings)
        {
            BlAsaasCustomer = new(settings);
            HubCustomerDAO = new(settings);
            LogHistoryDAO = new(settings);
            HubOrderDAO = new(settings);
            HubAllyDAO = new(settings);
            HubPaymentOrderDAO = new(settings);
            AsaasChargeService = new();
        }

        public async Task<HubOrderCreationChargeOutput> CreateCharge(string orderId, HubOrderInputPaymentData payments) => (await CreateCharges(orderId, new List<HubOrderInputPaymentData> { payments }).ConfigureAwait(false))?.FirstOrDefault();

        public async Task<List<HubOrderCreationChargeOutput>> CreateCharges(string orderId, List<HubOrderInputPaymentData> payments)
        {
            if (!ChargeValidation(orderId, payments))
                return null;

            var order = HubOrderDAO.FindById(orderId);
            var customer = HubCustomerDAO.FindById(order.Customer?.CustomerId);

            if (string.IsNullOrEmpty(customer.AsaasId))
                customer.AsaasId = (await BlAsaasCustomer.GetCustomerById(order.Customer?.CustomerId).ConfigureAwait(false))?.Customer?.Id;

            if (string.IsNullOrEmpty(customer.AsaasId))
                return null;

            var input = new AsaasCreateChargeInput(order, customer.AsaasId);
            var resultList = new List<HubOrderCreationChargeOutput>();
            foreach (var payment in payments)
            {
                if (payment.Type == HubOrderPaymentFormEnum.Money)
                {
                    resultList.Add(new(HubOrderPaymentFormsOutput.Money, HubAsaasPaymentStatusEnum.Confirmed, payment.Value));
                    continue;
                }

                input.BillingType = HubPaymentOrder.GetAsaasPaymentString(payment.Type);
                input.Value = payment.Value;

                if (payment.Type == HubOrderPaymentFormEnum.CreditCard && payment.CreditCard != null)
                {
                    input.CreditCard = new(payment.CreditCard.CardData);
                    input.CreditCardHolderInfo = new(payment.CreditCard.HolderInfo);
                }

                AsaasCreateChargeResultOutput result = null;
                HubOrderCreationChargeOutput paymentOutput = null;
                try
                {
                    result = await AsaasChargeService.CreateCharge(input);
                    paymentOutput = await GetChargeOutput(result, input.BillingType).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    LogHistoryDAO.Insert(new AppLogHistory
                    {
                        Message = "Erro ao gerar pagamento no Asaas!",
                        Type = AppLogTypeEnum.XApiAsaasRequestError,
                        ExceptionMessage = e.Message,
                        Method = "CreatePayment",
                        Date = DateTime.Now
                    });
                }
                finally
                {
                    if (paymentOutput != null)
                    {
                        paymentOutput.Value = payment.Value;
                        resultList.Add(paymentOutput);
                    }
                    else
                        resultList.Add(new(new AsaasDefaultErrorResult((result == null ?
                            "Não foi possível gerar a cobrança para a Forma de Pagamento: " :
                            "Não foi possível pegar os detalhes do pagamento para a Forma de Pagamento: ")
                            + input.BillingType), HubPaymentOrder.GetPaymentStringFromAsaas(input.BillingType), payment.Value));
                }
            }

            return resultList;
        }

        public async Task<AsaasCreateChargeResultOutput> GetChargeDetails(string input) => await AsaasChargeService.GetChargeDetails(input).ConfigureAwait(false);

        public static OrderAsaasData GetAsaasData(HubOrderCreationChargeOutput charge) => charge.PaymentForm switch
        {
            HubOrderPaymentFormsOutput.BankSlip => new OrderAsaasData(HubOrderPaymentFormEnum.BankSlip, charge.AsaasId, charge.BankSlip),
            HubOrderPaymentFormsOutput.CreditCard => new OrderAsaasData(HubOrderPaymentFormEnum.CreditCard, charge.AsaasId, charge.CreditCard),
            HubOrderPaymentFormsOutput.Pix => new OrderAsaasData(HubOrderPaymentFormEnum.Pix, charge.AsaasId, charge.Pix),
            _ => null
        };

        public async Task<HubOrderCreationChargeOutput> GetChargeOutput(AsaasCreateChargeResultOutput input, string asaasPaymentString)
        {
            if (input == null)
                return null;

            var paymentForm = HubPaymentOrder.GetPaymentStringFromAsaas(asaasPaymentString);
            if (input?.Error?.Errors?.Any() ?? false)
                return new(input.Error, paymentForm);

            if (paymentForm == HubOrderPaymentFormsOutput.BankSlip)
                return new HubOrderCreationChargeOutput(new HubBankSlipOutput(input.Charge.BankSlipUrl, input.Charge.InvoiceUrl), paymentForm, input.Charge)
                {
                    Pix = new(await AsaasChargeService.GetQrCode(input.Charge.Id), input.Charge.InvoiceUrl)
                };

            return paymentForm switch
            {
                HubOrderPaymentFormsOutput.CreditCard => new(new HubCreditCardOutput(input.Charge.InvoiceUrl, input.Charge.TransactionReceiptUrl), paymentForm, input.Charge),
                HubOrderPaymentFormsOutput.Pix => new(new HubPixOutput(await AsaasChargeService.GetQrCode(input.Charge.Id), input.Charge.InvoiceUrl), paymentForm, input.Charge),
                HubOrderPaymentFormsOutput.Money => new(paymentForm, HubAsaasPaymentStatusEnum.Confirmed),
                _ => null,
            };
        }

        private bool ChargeValidation(string orderId, List<HubOrderInputPaymentData> payments)
        {
            if (string.IsNullOrEmpty(orderId))
                return false;

            var order = HubOrderDAO.FindById(orderId);
            if (order == null)
                return false;

            var ally = HubAllyDAO.FindById(order.AllyId);
            if (ally?.ChargeType != HubAllyChargeTypeEnum.Integrated)
                return false;

            if (order.Price == null)
                return false;

            if (!(payments?.Any() ?? false))
                return false;

            var customer = HubCustomerDAO.FindById(order.Customer?.CustomerId);
            if (customer == null)
                return false;

            return true;
        }
    }
}