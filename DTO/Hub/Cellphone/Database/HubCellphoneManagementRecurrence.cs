using DTO.General.Base.Database;
using DTO.Hub.Cellphone.Enum;
using DTO.Integration.Surf.Recurrence.Enum;
using System;

namespace DTO.Hub.Cellphone.Database
{
    public class HubCellphoneManagementRecurrence : BaseData
    {
        public HubCellphoneManagementRecurrence(string managementId, string transactionId, HubCellphoneRecurrenceStatusEnum status)
        {
            CellphoneManagementId = managementId;
            Status = status;
            SurfRecurrenceTransactionId = transactionId;
            CreationDate = DateTime.Now;
        }
        public string CellphoneManagementId { get; set; }
        public string SurfRecurrenceTransactionId { get; set; }
        public HubCellphoneRecurrenceStatusEnum Status { get; set; }
        public DateTime CreationDate { get; set; }

        public static HubCellphoneRecurrenceStatusEnum GetStatusFromSurfReportStatus(SurfRecurrenceStatus status) => status switch
        {
            SurfRecurrenceStatus.Paid => HubCellphoneRecurrenceStatusEnum.Paid,
            SurfRecurrenceStatus.Defaulter => HubCellphoneRecurrenceStatusEnum.NotPaid,
            SurfRecurrenceStatus.AwaitingChargePayment => HubCellphoneRecurrenceStatusEnum.NotPaid,
            SurfRecurrenceStatus.Cancel => HubCellphoneRecurrenceStatusEnum.Canceled,
            _ => HubCellphoneRecurrenceStatusEnum.Unknown,
        };
    }
}
