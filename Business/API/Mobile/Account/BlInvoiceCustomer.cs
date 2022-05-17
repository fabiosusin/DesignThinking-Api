using Business.API.General.InvoiceCustomer;
using Business.API.Hub.Integration.Asaas.Customer;
using DAO.DBConnection;
using DAO.General.Invoice;
using DTO.General.Base.Api.Output;
using DTO.General.Invoice.Input;
using DTO.General.Invoice.Output;
using DTO.Mobile.Account.Input;
using DTO.Mobile.Account.Output;
using System;
using System.Linq;
using System.Threading.Tasks;
using Useful.Extensions;

namespace Business.API.Mobile.Account
{
    public class BlInvoiceCustomer
    {
        private readonly BlCustomerMsisdn BlCustomerMsisdn;
        private readonly InvoiceCustomerDAO InvoiceCustomerDAO;
        private readonly BlInvoiceCustomerGeneral BlInvoiceCustomerGeneral;
        public BlInvoiceCustomer(XDataDatabaseSettings settings)
        {
            BlCustomerMsisdn = new(settings);
            InvoiceCustomerDAO = new(settings);
            BlInvoiceCustomerGeneral = new(settings);
        }

        public async Task<AppInvoiceCustomerDetailsOutput> GetInvoiceUnpaid(string number, string mobileId, string allyId, bool currentMonth = true)
        {
            var basicValidation = BasicValidation(number, allyId);
            if (basicValidation.Success)
                return await GetInvoiceUnpaid(new(allyId, number), currentMonth).ConfigureAwait(false);

            basicValidation = BasicValidation(mobileId, allyId);
            if (!basicValidation.Success)
                return new(basicValidation.Message);

            var cellphonesOutput = BlCustomerMsisdn.GetCellphonesByMobileId(mobileId, allyId);
            return !cellphonesOutput.Success ?
                new(cellphonesOutput.Message) :
                await GetInvoiceUnpaid(new(allyId, cellphonesOutput.Cellphones.Select(x => x.Id), false), currentMonth).ConfigureAwait(false);
        }

        public async Task<AppInvoiceCustomerDetailsOutput> GetInvoiceDetails(string invoiceId, string allyId)
        {
            var basicValidation = BasicValidation(invoiceId, allyId);
            if (!basicValidation.Success)
                return new(basicValidation.Message);

            var invoice = InvoiceCustomerDAO.FindOne(x => x.Id == invoiceId && x.AllyId == allyId);
            if (invoice == null)
                return new("Fatura não encontrada");

            return new(invoice, await BlInvoiceCustomerGeneral.GetInvoiceCharge(invoice.AsaasId).ConfigureAwait(false));
        }

        public InvoiceCustomerListOutput List(AppInvoiceListInput input)
        {
            var basicValidation = BasicValidation(input?.Filters?.Number, input?.Filters?.AllyId);
            if (basicValidation.Success)
                return BlInvoiceCustomerGeneral.List(new(new(input.Filters.AllyId, input.Filters.Number), input.Paginator));

            var cellphonesOutput = BlCustomerMsisdn.GetCellphonesByMobileId(input?.Filters?.MobileId, input?.Filters?.AllyId);
            if (!cellphonesOutput.Success)
                return new(cellphonesOutput.Message);

            if (string.IsNullOrEmpty(input.Filters.Number) && !(cellphonesOutput.Cellphones?.Any() ?? false))
                return new(false);

            return BlInvoiceCustomerGeneral.List(new(new(input.Filters.AllyId, cellphonesOutput.Cellphones.Select(x => x.Id), null), input.Paginator));
        }

        private async Task<AppInvoiceCustomerDetailsOutput> GetInvoiceUnpaid(InvoiceCustomerFiltersInput filters, bool currentMonth)
        {
            var date = DateTime.Now;
            var input = new InvoiceCustomerListInput(filters, 1, 1);
            if (currentMonth)
            {
                input.Filters.StartDate = date.Date;
                input.Filters.EndDate = DateTimeExtension.GetLastDayOfTheMonth(date);
            }
            else
                input.Filters.EndDate = date.AddMilliseconds(-1);

            if (string.IsNullOrEmpty(input.Filters.Number) && !(input.Filters.CellphonesManagementIds?.Any() ?? false))
                return new(false);

            var invoicesList = BlInvoiceCustomerGeneral.List(input);
            if (!invoicesList.Success)
                return new(invoicesList.Message);

            var invoice = invoicesList.Invoices?.FirstOrDefault();
            return new(invoice, await BlInvoiceCustomerGeneral.GetInvoiceCharge(invoice.AsaasId).ConfigureAwait(false));
        }

        private static BaseApiOutput BasicValidation(string registerId, string allyId)
        {
            if (string.IsNullOrEmpty(registerId))
                return new("RegisterId não informado!");

            if (string.IsNullOrEmpty(allyId))
                return new("AllyId não informado!");

            return new(true);
        }
    }
}
