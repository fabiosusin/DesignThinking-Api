using DTO.General.Base.Api.Output;
using DTO.General.Invoice.Database;
using DTO.Hub.Cellphone.Enum;
using System;
using System.Collections.Generic;

namespace DTO.Hub.Cellphone.Output
{
    public class HubRecurrenceListOutput : BaseApiOutput
    {
        public HubRecurrenceListOutput(string msg) : base(msg) { }
        public HubRecurrenceListOutput(IEnumerable<HubRecurrenceListData> recurrences) : base(true) => Numbers = recurrences;
        public IEnumerable<HubRecurrenceListData> Numbers { get; set; }
    }

    public class HubRecurrenceListData
    {
        public string Id { get; set; }
        public string CustomerName { get; set; }
        public string Number { get; set; }
        public decimal SurfPrice { get; set; }
        public decimal OrderPrice { get; set; }
        public HubCellphoneManagementStatusEnum Status { get; set; }
        public HubCellphoneManagementTypeEnum Mode { get; set; }
        public DateTime CreationDate { get; set; }
        public IEnumerable<HubRecurrence> Recurrences { get; set; }
        public IEnumerable<HubInvoiceCustomer> Invoices { get; set; }
    }

    public class HubInvoiceCustomer
    {
        public HubInvoiceCustomer(InvoiceCustomer input)
        {
            if (input == null)
                return;

            InvoiceId = input.Id;
            Paid = input.Paid;
            CreationDate = input.CreationDate;
            ExpirationDate = input.ExpirationDate;
            PayIn = input.PayIn;
            Value = input.Value;
        }

        public string InvoiceId { get; set; }
        public decimal Value { get; set; }
        public bool Paid { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime PayIn { get; set; }
    }

    public class HubRecurrence
    {
        public HubRecurrence(DateTime date, HubCellphoneRecurrenceStatusEnum status)
        {
            Date = date;
            Status = status;
        }

        public DateTime Date { get; set; }
        public HubCellphoneRecurrenceStatusEnum Status { get; set; }
    }
}
