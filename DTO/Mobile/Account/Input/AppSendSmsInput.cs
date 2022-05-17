using DTO.Mobile.Account.Enum;

namespace DTO.Mobile.Account.Input
{
    public class AppSendSmsInput
    {
        public AppSendSmsInput() { }
        public AppSendSmsInput(string sender, string body, string mobileId, AppSmsTypeEnum type)
        {
            Sender = sender;
            Body = body;
            MobileId = mobileId;
            Type = type;
        }

        public string Sender { get; set; }
        public string Body { get; set; }
        public string MobileId { get; set; }
        public AppSmsTypeEnum Type { get; set; }
    }
}
