using DTO.Integration.Surf.Token.Input;

namespace DTO.Integration.Surf.AccountPlan.Input
{
    public class SurfFreeUsageInfoInput : SurfDetailsBaseApiInput
    {
        public SurfFreeUsageInfoInput() { }
        public SurfFreeUsageInfoInput(string msisdn, string bundleCode)
        {
            MSISDN = msisdn;
            BundleCode = bundleCode;
        }
        public string BundleCode { get; set; }
    }
}
