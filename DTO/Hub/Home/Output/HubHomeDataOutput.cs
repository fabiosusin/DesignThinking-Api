using DTO.General.Base.Api.Output;
using DTO.General.Home;
using System;

namespace DTO.Hub.Home.Output
{
    public class HubHomeDataOutput : BaseApiOutput
    {
        public HubHomeDataOutput(string msg) : base(msg) { }

        public HubHomeDataOutput(decimal allyQuantity, decimal orderQuantity, decimal customerQuantity) : base(true)
        {
            Ally = new(allyQuantity);
            Order = new(orderQuantity);
            Customer = new(customerQuantity);
        }

        public HomeDataItemInfo Ally { get; set; }
        public HomeDataItemInfo Order { get; set; }
        public HomeDataItemInfo Customer { get; set; }
    }
}
