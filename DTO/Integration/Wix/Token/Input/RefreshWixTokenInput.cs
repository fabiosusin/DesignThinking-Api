using Newtonsoft.Json;

namespace DTO.Integration.Wix.Token.Input
{
    public class RefreshWixTokenInput
    {
        public RefreshWixTokenInput(string clientId, string clientSecret, string refreshToken)
        {
            GrantType = "refresh_token";
            ClientId = clientId;
            ClientSecret = clientSecret;
            RefreshToken = refreshToken;
        }

        [JsonProperty("grant_type")]
        public string GrantType { get; set; }

        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
