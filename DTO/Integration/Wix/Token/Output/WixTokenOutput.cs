using Newtonsoft.Json;

namespace DTO.Integration.Wix.Token.Output
{
    public class WixTokenOutput
    {
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}
