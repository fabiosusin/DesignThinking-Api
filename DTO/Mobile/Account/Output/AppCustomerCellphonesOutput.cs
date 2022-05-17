using DTO.General.Base.Api.Output;
using DTO.Hub.Cellphone.Database;
using System.Collections.Generic;

namespace DTO.Mobile.Account.Output
{
    public class AppCustomerCellphonesOutput : BaseApiOutput
    {
        public AppCustomerCellphonesOutput(string msg) : base(msg) { }
        public AppCustomerCellphonesOutput(IEnumerable<HubCellphoneManagement> cellphones) : base(true) => Cellphones = cellphones;

        public IEnumerable<HubCellphoneManagement> Cellphones { get; set; }
    }
}
