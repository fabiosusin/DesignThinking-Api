using DTO.General.Base.Api.Output;
using DTO.General.Surf.Database;
using System.Collections.Generic;

namespace DTO.Mobile.Account.Output
{
    public class AppCustomerMsisdOutput : BaseApiOutput
    {
        public AppCustomerMsisdOutput(string msg) : base(msg) { }
        public AppCustomerMsisdOutput(IEnumerable<SurfCustomerMsisdn> cellphones) : base(true) => Cellphones = cellphones;

        public IEnumerable<SurfCustomerMsisdn> Cellphones { get; set; }
    }
}
