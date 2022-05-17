using DTO.General.Base.Api.Output;
using DTO.General.Invoice.Database;
using System.Collections.Generic;

namespace DTO.General.Invoice.Output
{
    public class InvoiceCustomerListOutput : BaseApiOutput
    {
        public InvoiceCustomerListOutput(string msg) : base(msg) { }
        public InvoiceCustomerListOutput(bool scs) : base(scs) { }
        public InvoiceCustomerListOutput(IEnumerable<InvoiceCustomer> invoices) : base(true) => Invoices = invoices;
        public IEnumerable<InvoiceCustomer> Invoices { get; set; }
    }
}
