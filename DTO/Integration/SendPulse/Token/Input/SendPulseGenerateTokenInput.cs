using Newtonsoft.Json;

namespace DTO.Integration.SendPulse.Token.Input
{
    public class SendPulseGenerateTokenInput
    {
        public SendPulseGenerateTokenInput(string type, string id, string secret)
        {
            GrantType = type;
            ClientId = id;
            ClientSecret = secret;
        }

        [JsonProperty("grant_type")]
        public string GrantType { get; set; }

        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }
    }
}
