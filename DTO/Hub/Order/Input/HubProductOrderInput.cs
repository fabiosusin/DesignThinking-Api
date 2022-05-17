using DTO.Hub.Cellphone.Database;
using DTO.Hub.Cellphone.Enum;
using DTO.Hub.Order.Database;
using DTO.Hub.Product.Database;

namespace DTO.Hub.Order.Input
{
    public class HubProductOrderInput
    {
        public HubProductOrderInput() { }
        public HubProductOrderInput(HubProductOrder productOrder, HubProduct product)
        {
            if (productOrder == null && product == null)
                return;

            Code = product.Code;
            Name = product.Name;
            ProductId = product.Id;
            CategoryId = product.CategoryId;
            Quantity = productOrder.Quantity;
            Price = productOrder.Price;
            CellphoneData = productOrder.CellphoneData;
        }

        public string Code { get; set; }
        public string Name { get; set; }
        public string ProductId { get; set; }
        public string CategoryId { get; set; }
        public string BitDefenderCategoryId { get; set; }
        public decimal Quantity { get; set; }
        public HubProductOrderCellphoneData CellphoneData { get; set; }
        public HubProductPriceTablePrice Price { get; set; }
    }

    public class HubProductOrderBitDefenderData
    {
        public string LicenseId { get; set; }
    }

    public class HubProductOrderCellphoneData
    {
        public string ChipSerial { get; set; }
        public string DDD { get; set; }
        public string SurfMobilePlanId { get; set; }
        public HubCellphoneManagementTypeEnum Mode { get; set; }
        public CellphoneManagementPortability Portability { get; set; }
    }
}
