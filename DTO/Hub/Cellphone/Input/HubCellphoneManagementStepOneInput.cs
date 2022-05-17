using DTO.Hub.Cellphone.Database;
using DTO.Hub.Cellphone.Enum;
using DTO.Hub.Order.Input;

namespace DTO.Hub.Cellphone.Input
{
    public class HubCellphoneManagementStepOneInput
    {
        public HubCellphoneManagementStepOneInput() { }

        public HubCellphoneManagementStepOneInput(HubCellphoneManagement management)
        {
            if (management == null)
                return;

            ChipSerial = management.ChipSerial;
            CustomerId = management.CustomerId;
            DDD = management.CellphoneData?.DDD;
            Mode = management.Mode;
            OrderId = management.OrderId;
            ProductOrderId = management.ProductOrderId;
            Portability = management.Portability;
            Price = management.Price?.OrderPrice ?? 0;
            SurfMobilePlanId = management.SurfMobilePlanId;
        }

        public HubCellphoneManagementStepOneInput(HubProductOrderCellphoneData input, string allyId, string orderId, string productOrderId, string customerId, string surfPlanId, decimal price)
        {
            if (input== null)
                return;

            AllyId = allyId;
            SurfMobilePlanId = surfPlanId;
            ChipSerial = input.ChipSerial;
            DDD = input.DDD;
            Portability = input.Portability;
            Mode = input.Mode;
            OrderId = orderId;
            ProductOrderId = productOrderId;
            CustomerId = customerId;
            Price = price;
        }

        public string AllyId { get; set; }
        public string SurfMobilePlanId { get; set; }
        public string OrderId { get; set; }
        public string ProductOrderId { get; set; }
        public string ChipSerial { get; set; }
        public string CustomerId { get; set; }
        public string DDD { get; set; }
        public decimal Price { get; set; }
        public CellphoneManagementPortability Portability { get; set; }
        public HubCellphoneManagementTypeEnum Mode { get; set; }
    }
}