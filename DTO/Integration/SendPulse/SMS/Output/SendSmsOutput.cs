using Newtonsoft.Json;

namespace DTO.Integration.SendPulse.SMS.Output
{

    public class SendSmsOutput
    {
        [JsonProperty("campaign_id")]
        public int CampaignId { get; set; }
        public bool Result { get; set; }
        public Counters Counters { get; set; }
    }

    public class Counters
    {
        public int Exceptions { get; set; }
        public int Sends { get; set; }
    }
}
