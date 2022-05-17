using DTO.Integration.Surf.Token.Input;

namespace DTO.Integration.Surf.AccountPlan.Input
{
    public class SurfBundleInfoInput : SurfDetailsBaseApiInput
    {
        public SurfBundleInfoInput() { }
        public SurfBundleInfoInput(string msisdn) : base(msisdn) { }
    }
}
