using DTO.General.Base.Database;

namespace DTO.Mobile.Surf.Database
{
    public class SurfMobilePlan : BaseData
    {

        public SurfData SurfData { get; set; }
        public string Name { get; set; }
        public decimal? InternetLimit { get; set; }
        public decimal? SmsLimit { get; set; }
        public decimal? CallLimit { get; set; }
    }

    public class SurfData
    {
        public string Id { get; set; }
        public SurfDataPrice Price { get; set; }
    }

    public class SurfDataPrice
    {
        public SurfDataPrice(decimal cost, decimal price)
        {
            Cost = cost;
            Price = price;
        }
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
    }
}
