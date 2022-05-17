using System;
using System.Collections.Generic;

namespace DTO.General.Invoice.Input
{
    public class InvoiceCustomerFiltersInput
    {
        public InvoiceCustomerFiltersInput() { }
        public InvoiceCustomerFiltersInput(IEnumerable<string> ids) => CellphonesManagementIds = ids;

        public InvoiceCustomerFiltersInput(string allyId, IEnumerable<string> ids, bool? paid = null)
        {
            AllyId = allyId;
            Paid = paid;
            CellphonesManagementIds = ids;
        }

        public InvoiceCustomerFiltersInput(string allyId, string number)
        {
            AllyId = allyId;
            Number = number;
        }

        public string AllyId { get; set; }
        public string Number { get; set; }
        public string HubCustomerId { get; set; }
        public bool? Paid { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public IEnumerable<string> CellphonesManagementIds { get; set; }
    }
}
