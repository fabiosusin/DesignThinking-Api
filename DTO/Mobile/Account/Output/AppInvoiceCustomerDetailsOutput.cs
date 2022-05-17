using DTO.General.Base.Api.Output;
using DTO.General.Invoice.Database;
using DTO.Hub.Order.Output;

namespace DTO.Mobile.Account.Output
{
    public class AppInvoiceCustomerDetailsOutput : BaseApiOutput
    {
        public AppInvoiceCustomerDetailsOutput(string msg) : base(msg) { }
        public AppInvoiceCustomerDetailsOutput(bool scs) : base(scs) { }
        public AppInvoiceCustomerDetailsOutput(InvoiceCustomer invoices) : base(true) => Details = new(invoices);
        public AppInvoiceCustomerDetailsOutput(InvoiceCustomer invoices, HubOrderCreationChargeOutput charge) : base(true) => Details = new(invoices, charge);
        public AppInvoiceCustomerOutput Details { get; set; }
    }

    public class AppInvoiceCustomerOutput
    {
        public AppInvoiceCustomerOutput(InvoiceCustomer invoices) => InvoiceCustomer = invoices;
        public AppInvoiceCustomerOutput(InvoiceCustomer invoices, HubOrderCreationChargeOutput charge)
        {
            InvoiceCustomer = invoices;
            Charge = charge;
        }

        public InvoiceCustomer InvoiceCustomer { get; set; }
        public HubOrderCreationChargeOutput Charge { get; set; }
    }
}
