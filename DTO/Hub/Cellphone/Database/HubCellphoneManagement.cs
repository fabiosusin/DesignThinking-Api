using DTO.General.Base.Database;
using DTO.Mobile.Surf.Database;
using DTO.Hub.Cellphone.Input;
using System;
using DTO.Hub.Cellphone.Enum;
using DTO.Integration.Surf.Recurrence.Enum;

namespace DTO.Hub.Cellphone.Database
{
    public class HubCellphoneManagement : BaseData
    {
        public HubCellphoneManagement() { }

        public HubCellphoneManagement(HubCellphoneManagementStepOneInput input, SurfData surfData)
        {
            if (input == null)
                return;

            AllyId = input.AllyId;
            SurfMobilePlanId = input.SurfMobilePlanId;
            CustomerId = input.CustomerId;
            OrderId = input.OrderId;
            ChipSerial = input.ChipSerial;
            SurfPlanId = surfData?.Id;
            Mode = input.Mode;
            Portability = input.Portability;
            Price = new(surfData?.Price?.Cost ?? 0, surfData?.Price?.Price ?? 0);
            CellphoneData = new(input.DDD);
            ProductOrderId = input.ProductOrderId;
            CreationDate = LastUpdate = DateTime.Now;
            Status = HubCellphoneManagementStatusEnum.Create;
        }

        public string CustomerId { get; set; }
        public string SurfCustomerId { get; set; }
        public string SurfMobilePlanId { get; set; }
        public string SurfPlanId { get; set; }
        public string OrderId { get; set; }
        public string AllyId { get; set; }
        public string ProductOrderId { get; set; }
        public string SurfTransactionId { get; set; }
        public string ChipSerial { get; set; }
        public HubCellphoneManagementTypeEnum Mode { get; set; }
        public HubCellphoneManagementStatusEnum Status { get; set; }
        public DateTime RecurrenceDate { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public SurfCellphoneData CellphoneData { get; set; }
        public CellphoneManagementPortability Portability { get; set; }
        public CellphoneManagementPrice Price { get; set; }

        public static HubCellphoneManagementStatusEnum GetStatusFromSurfReportStatus(SurfRecurrenceStatus status) => status switch
        {
            SurfRecurrenceStatus.Paid => HubCellphoneManagementStatusEnum.Completed,
            SurfRecurrenceStatus.Defaulter => HubCellphoneManagementStatusEnum.Defaulter,
            SurfRecurrenceStatus.AwaitingChargePayment => HubCellphoneManagementStatusEnum.AwaitingChargePayment,
            SurfRecurrenceStatus.Cancel => HubCellphoneManagementStatusEnum.Canceled,
            _ => HubCellphoneManagementStatusEnum.Unknown,
        };
    }

    public class SurfCellphoneData
    {
        public SurfCellphoneData(string input)
        {
            if (string.IsNullOrEmpty(input))
                return;

            if (input.Length == 13)
            {
                CountryPrefix = input[..2];
                DDD = input.Substring(2, 2);
                Number = input[4..];
            }

            if (input.Length == 11)
            {
                DDD = input[..2];
                Number = input[2..];
            }

            if (input.Length == 9)
                Number = input;

            if (input.Length == 2)
                DDD = input;
        }

        public SurfCellphoneData() { }

        public string Number { get; set; }
        public string CountryPrefix { get; set; }
        public string DDD { get; set; }
    }

    public class CellphoneManagementPortability
    {
        public string Number { get; set; }
        public string OperatorId { get; set; }
    }

    public class CellphoneManagementPrice
    {
        public CellphoneManagementPrice() { }
        public CellphoneManagementPrice(decimal surf, decimal order)
        {
            SurfPlanPrice = surf;
            OrderPrice = order;
        }
        public decimal SurfPlanPrice { get; set; }
        public decimal OrderPrice { get; set; }
    }
}
