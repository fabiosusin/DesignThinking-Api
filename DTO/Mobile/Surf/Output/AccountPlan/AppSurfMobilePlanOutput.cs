using DTO.Integration.Surf.BaseDetails.Output;

namespace DTO.Surf.Output.AccountPlan
{
    public class AppSurfMobilePlanOutput : SurfDetailsBaseApiOutput
    {
        public AppSurfMobilePlanOutput () { }
        public AppSurfMobilePlanOutput (string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public AccountData Internet { get; set; }
        public AccountData Call { get; set; }
        public AccountData Sms { get; set; }
    }

    public class AccountData
    {
        public decimal? Limit { get; set; }
        public decimal ToUse { get; set; }
    }
}
