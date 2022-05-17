using DTO.Integration.Surf.Token.Input;

namespace DTO.Integration.Surf.AccountDetails.Input
{
    public class SurfAccountDetailsInput : SurfDetailsBaseApiInput
    {
        public SurfAccountDetailsInput() { }
        public SurfAccountDetailsInput(string msisdn) : base(msisdn) { }
    }
}
