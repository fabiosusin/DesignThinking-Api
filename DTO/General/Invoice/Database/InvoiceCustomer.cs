using DTO.General.Base.Database;
using DTO.Hub.Order.Output;
using System;

namespace DTO.General.Invoice.Database
{
    public class InvoiceCustomer : BaseData
    {
        public InvoiceCustomer() { }
        public InvoiceCustomer(HubOrderCreationChargeOutput charge, string hubCustomer, string cellphoneManagementId, string allyId)
        {
            HubCustomerId = hubCustomer;
            CellphoneManagementId = cellphoneManagementId;
            AllyId = allyId;
            CreationDate = DateTime.Now;
            ExpirationDate = DateTime.Now.AddDays(10);

            if (charge == null)
                return;

            AsaasId = charge.AsaasId;
            Value = charge.Value;
        }

        public string HubCustomerId { get; set; }
        public string AllyId { get; set; }
        public string CellphoneManagementId { get; set; }
        public string AsaasId { get; set; }
        public bool Paid { get; set; }
        public decimal Value { get; set; }
        public DateTime PayIn { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
