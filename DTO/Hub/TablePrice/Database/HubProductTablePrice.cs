using DTO.General.Base.Database;

namespace DTO.Hub.TablePrice.Database
{
    public class HubProductTablePrice : BaseData
    {
        public string ProductId { get; set; }
        public string TablePriceId { get; set; }
        public ProductTablePrice_Price Price { get; set; }
    }

    public class ProductTablePrice_Price
    {
        public decimal Cost { get; set; }
        public decimal Sale { get; set; }
    }
}
