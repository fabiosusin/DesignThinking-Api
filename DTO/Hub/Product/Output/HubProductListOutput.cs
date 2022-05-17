using DTO.General.Base.Api.Output;
using DTO.Hub.Product.Database;
using System.Collections.Generic;

namespace DTO.Hub.Product.Output
{
    public class HubProductListOutput : BaseApiOutput
    {
        public HubProductListOutput(string msg) : base(msg) { }
        public HubProductListOutput(IEnumerable<HubProduct> products) : base(true) => Products = products;
        public IEnumerable<HubProduct> Products { get; set; }
    }
}
