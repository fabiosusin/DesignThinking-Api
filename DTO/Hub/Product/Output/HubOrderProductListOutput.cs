using DTO.General.Base.Api.Output;
using DTO.Hub.Product.Database;
using DTO.Mobile.Surf.Database;
using System.Collections.Generic;

namespace DTO.Hub.Product.Output
{
    public class HubOrderProductListOutput : BaseApiOutput
    {
        public HubOrderProductListOutput(string msg) : base(msg) { }
        public HubOrderProductListOutput(IEnumerable<HubProductList> products) : base(true) => Products = products;
        public IEnumerable<HubProductList> Products { get; set; }
    }

    public class HubProductList : HubProduct
    {
        public HubProductList(HubProduct input) : base(input) { }
        public SurfMobilePlan MobilePlan { get; set; }
    }

    public class BitDefenderCategoryListOutput
    {
        public BitDefenderCategoryListOutput(string name) => Name = name;
        public string Name { get; set; }
    }
}
