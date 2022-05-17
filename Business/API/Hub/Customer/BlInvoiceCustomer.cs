using Business.API.General.InvoiceCustomer;
using Business.API.Mobile.Account;
using DAO.DBConnection;
using DAO.General.Invoice;
using DAO.General.Log;
using DAO.General.Surf;
using DAO.Hub.UserDAO;
using DTO.General.Base.Api.Output;
using DTO.General.Invoice.Database;
using DTO.General.Log.Database;
using DTO.General.Log.Enum;
using DTO.Hub.Order.Output;
using DTO.Mobile.Account.Enum;
using Services.Integration.Asaas.Customer;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Business.API.Hub.Customer
{
    public class BlInvoiceCustomer
    {
        private readonly HubUserDAO HubUserDAO;
        private readonly BlDefaultSms BlDefaultSms;
        private readonly LogHistoryDAO LogHistoryDAO;
        private readonly AsaasChargeService AsaasChargeService;
        private readonly InvoiceCustomerDAO InvoiceCustomerDAO;
        private readonly SurfCustomerMsisdnDAO SurfCustomerMsisdnDAO;
        private readonly BlInvoiceCustomerGeneral BlInvoiceCustomerGeneral;

        public BlInvoiceCustomer(XDataDatabaseSettings settings)
        {
            HubUserDAO = new(settings);
            AsaasChargeService = new();
            BlDefaultSms = new(settings);
            LogHistoryDAO = new(settings);
            InvoiceCustomerDAO = new(settings);
            SurfCustomerMsisdnDAO = new(settings);
            BlInvoiceCustomerGeneral = new(settings);
        }

        public async Task<HubOrderCreationChargeOutput> GetChargeDetails(string invoiceId) =>
            await BlInvoiceCustomerGeneral.GetInvoiceCharge(InvoiceCustomerDAO.FindById(invoiceId)?.AsaasId).ConfigureAwait(false);

        public async Task<BaseApiOutput> PaidInvoice(string invoiceId, string userId)
        {
            var invoice = InvoiceCustomerDAO.FindById(invoiceId);
            if (invoice == null)
                return new("Fatura não encontrada!");

            var user = HubUserDAO.FindById(userId);
            if (user == null)
                return new("Usuário não encontrado!");

            if (user.AllyId != invoice.AllyId)
                return new("Usuário não possui permissão para marcar fatura como Paga!");

            try
            {
                var result = await AsaasChargeService.CancelCharge(invoice.AsaasId).ConfigureAwait(false);
                if (result == null || (result.Error?.Errors?.Any() ?? false))
                    return new("Não foi possível marcar a fatura como paga!");

                InvoiceCustomerDAO.PaidInvoice(invoiceId);
                LogHistoryDAO.Insert(new AppLogHistory
                {
                    Message = $"Fatura {invoice.Id} paga pelo usuário {user.Name}!",
                    Type = AppLogTypeEnum.XApiInfo,
                    Method = "PaidInvoice",
                    Date = DateTime.Now
                });
            }
            catch { }

            return new(true);
        }

        public async Task<BaseApiOutput> SaveInvoice(InvoiceCustomer invoice)
        {
            var validation = ValidateInvoice(invoice);
            if (!validation.Success)
                return validation;

            InvoiceCustomerDAO.Insert(invoice);

            var smsResult = await SendSmsToCustomer(invoice.CellphoneManagementId, "Sua fatura do plano celular foi gerada com vencimento em: " + invoice.ExpirationDate.Date).ConfigureAwait(false);
            if (!smsResult.Success)
                return smsResult;

            return new(true);
        }

        public async Task<BaseApiOutput> SendSmsToCustomer(string cellphoneManagementId, string body)
        {
            if (string.IsNullOrEmpty(cellphoneManagementId))
                return new("Gerenciamento Telefônico não informado!");

            var msisdn = SurfCustomerMsisdnDAO.FindOne(x => x.CellphoneManagementId == cellphoneManagementId);
            if (msisdn == null)
                return new("Gerenciamento Telefônico não encontrado!");

            return await BlDefaultSms.SendSms(new("XPlay - Cobranças", body, msisdn.Number, AppSmsTypeEnum.Default)).ConfigureAwait(false);
        }

        private static BaseApiOutput ValidateInvoice(InvoiceCustomer invoice)
        {
            if (invoice == null)
                return new("Fatura não informada");

            if (string.IsNullOrEmpty(invoice.HubCustomerId))
                return new("Cliente não informado");

            if (string.IsNullOrEmpty(invoice.AllyId))
                return new("Aliado não informado");

            if (string.IsNullOrEmpty(invoice.AsaasId))
                return new("Cobrança do Asaas não informada");

            if (string.IsNullOrEmpty(invoice.CellphoneManagementId))
                return new("Gerenciamento Telefônico não informado");

            return new(true);
        }
    }
}
