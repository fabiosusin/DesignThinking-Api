using DTO.General.Base.Api.Output;
using DTO.General.Base.Output;
using System.Collections.Generic;

namespace DTO.Hub.BitDefender.Output
{

    public class BitDefenderUseLicensesOutput : BaseApiOutput
    {
        public BitDefenderUseLicensesOutput(string error) : base(error) { }
        public BitDefenderUseLicensesOutput(BitDefenderLicensesOutput data) : base(true) => Data = data;
        public BitDefenderLicensesOutput Data { get; set; }
    }

    public class BitDefenderLicensesOutput
    {
        public BitDefenderLicensesOutput(string id, string name, IEnumerable<string> licenses)
        {
            Category = new(id, name);
            Licenses = licenses;
        }
        public BaseInfoOutput Category { get; set; }
        public IEnumerable<string> Licenses { get; set; }
    }
}
