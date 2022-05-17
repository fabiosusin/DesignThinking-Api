using DTO.Integration.Surf.Token.Input;
using DTO.Mobile.Account.Input;

namespace DTO.Integration.Surf.SMS.Input
{
    public class SurfValidateSmsOtpInput : SurfDetailsBaseApiInput
    {
        public SurfValidateSmsOtpInput() { }
        public SurfValidateSmsOtpInput(AppValidateOtpInput input, string msisdn)
        {
            if (input == null)
                return;

            Value = input.Code;
            MSISDN = msisdn ?? input.MobileId;
        }

        public SurfValidateSmsOtpInput(string msisdn, string value) : base(msisdn)
        {
            Value = value;
        }

        public string Value { get; set; }
    }
}
