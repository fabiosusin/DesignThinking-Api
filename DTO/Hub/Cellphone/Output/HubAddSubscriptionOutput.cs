using DTO.General.Base.Api.Output;

namespace DTO.Hub.Cellphone.Output
{
    public class HubAddSubscriptionOutput : BaseApiOutput
    {
        public HubAddSubscriptionOutput(bool success) : base(success) { }
        public HubAddSubscriptionOutput(string msg) : base(msg) { }
        public HubAddSubscriptionOutput(string msg, string id) : base(msg) => ManagementId = id;

        public string ManagementId { get; set; }
        public string ICCID { get; set; }
        public string Number { get; set; }
    }
}
