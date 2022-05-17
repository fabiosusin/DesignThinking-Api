using DTO.General.Base.Database;

namespace DTO.Hub.Product.Database
{
    public class HubProductPriceTable : BaseData
    {
        public string TablePriceId { get; set; }
        public string ProductId { get; set; }
        public HubProductPriceTablePrice Price { get; set; }
    }

    public class HubProductPriceTablePrice
    {
        public decimal Cost { get; set; }
        public decimal XPlayShare { get; set; }
        public decimal AllyShare { get; set; }
        public decimal Tax { get; set; }
        public decimal Price { get; set; }
        public decimal ChipPrice { get; set; }
    }
}
