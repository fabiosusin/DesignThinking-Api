using Business.API.Hub.Integration.Asaas.Customer;
using DAO.DBConnection;
using DAO.General.Invoice;
using DTO.General.Invoice.Input;
using DTO.General.Invoice.Output;
using DTO.Hub.Order.Output;
using System.Linq;
using System.Threading.Tasks;

namespace Business.API.General.InvoiceCustomer
{
    public class BlInvoiceCustomerGeneral
    {
        private readonly BlAsaasCharge BlAsaasCharge;
        private readonly InvoiceCustomerDAO InvoiceCustomerDAO;

        public BlInvoiceCustomerGeneral(XDataDatabaseSettings settings)
        {
            BlAsaasCharge = new(settings);
            InvoiceCustomerDAO = new(settings);
        }
        
        public InvoiceCustomerListOutput List(InvoiceCustomerListInput input)
        {
            var result = InvoiceCustomerDAO.List(input);
            if (!(result?.Any() ?? false))
                return new("Nenhuma Fatura encontrada!");

            return new(result);
        }

        public async Task<HubOrderCreationChargeOutput> GetInvoiceCharge(string asaasId)
        {
            var asaasCharge = await BlAsaasCharge.GetChargeDetails(asaasId).ConfigureAwait(false);
            return await BlAsaasCharge.GetChargeOutput(asaasCharge, asaasCharge?.Charge?.BillingType).ConfigureAwait(false);
        }
    }
}
