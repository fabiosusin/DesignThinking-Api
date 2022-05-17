using DTO.Mobile.Account.Enum;
using DTO.Mobile.Account.Input;
using Newtonsoft.Json;

namespace DTO.Integration.Surf.SMS.Input
{
    public class SurfSendSmsOtpInput
    {
        public SurfSendSmsOtpInput() { }
        public SurfSendSmsOtpInput(AppSendSmsInput input, string msisdn)
        {
            if (input == null)
                return;

            MSISDN = msisdn ?? input.MobileId;
            Text = input.Body;
            TokenValidate = input.Type == AppSmsTypeEnum.Otp;
        }

        public string TransactionID { get; set; }
        public string MSISDN { get; set; }
        public string Text { get; set; }

        [JsonProperty("tokenValidate")]
        public string TokenValidateStr { get => TokenValidate ? "True" : "False"; set { } }

        [JsonIgnore]
        public bool TokenValidate { get; set; }
    }
}
