using DTO.External.BitDefender.Database;
using DTO.General.Base.Api.Output;
using System.Collections.Generic;

namespace DTO.Hub.BitDefender.Output
{
    public class HubBitDefenderLicensesListOutput : BaseApiOutput
    {
        public HubBitDefenderLicensesListOutput(string msg) : base(msg) { }
        public HubBitDefenderLicensesListOutput(IEnumerable<HubBitDefenderLicenseListData> licenses) : base(true) => Licenses = licenses;
        public IEnumerable<HubBitDefenderLicenseListData> Licenses { get; set; }
    }

    public class HubBitDefenderLicenseListData : BitDefenderLicense
    {
        public HubBitDefenderLicenseListData() { }
        public HubBitDefenderLicenseListData(BitDefenderLicense input) 
        {
            if (input == null)
                return;

            BitDefenderCategoryId = input.BitDefenderCategoryId;
            Key = input.Key;
            AllyId = input.AllyId;
            OrderId = input.OrderId;
            Used = input.Used;
            RegisterDate = input.RegisterDate;
            LastUpdate = input.LastUpdate;
        }
        
        public long OrderCode { get; set; }
        public string CategoryName { get; set; }
    }
}
