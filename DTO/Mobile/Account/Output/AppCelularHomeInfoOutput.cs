using DTO.General.Base.Api.Output;
using DTO.Surf.Output.AccountPlan;
using System.Collections.Generic;

namespace DTO.Mobile.Account.Output
{
    public class AppCelularHomeInfoOutput : BaseApiOutput
    {
        public AppCelularHomeInfoOutput(string msg) : base(msg) { }
        public AppCelularHomeInfoOutput(List<MobileCellphoneData> cellphones) : base(true) => Cellphones = cellphones;

        public List<MobileCellphoneData> Cellphones { get; set; }
    }

    public class MobileCellphoneData
    {
        public string PlanName { get; set; }
        public string MsisdnFormatted { get; set; }
        public long Msisdn { get; set; }
        public AccountData Call { get; set; }
        public AccountData Internet { get; set; }
        public AccountData Sms { get; set; }
    }
}
