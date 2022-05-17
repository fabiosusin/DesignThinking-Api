using DTO.General.Base.Database;
using DTO.Hub.Order.Input;
using DTO.Hub.Product.Database;

namespace DTO.Hub.Order.Database
{
    public class HubProductOrder : BaseData
    {
        public HubProductOrder() { }
        public HubProductOrder(string orderId, string productId, HubProductOrderInput product)
        {
            OrderId = orderId;
            ProductId = productId;
            if (product == null)
                return;

            Quantity = product.Quantity;
            Price = product.Price;
            CellphoneData = product.CellphoneData;
            CategoryId = product.CategoryId;
            BitDefenderCategoryId = product.BitDefenderCategoryId;
            SurfMobilePlanId = product.CellphoneData?.SurfMobilePlanId;
        }

        public string BitDefenderCategoryId { get; set; }
        public string SurfMobilePlanId { get; set; }
        public string OrderId { get; set; }
        public string ProductId { get; set; }
        public string CategoryId { get; set; }
        public decimal Quantity { get; set; }
        public HubProductPriceTablePrice Price { get; set; }
        public HubProductOrderCellphoneData CellphoneData { get; set; }
    }
}
