using Newtonsoft.Json;

namespace DTO.Integration.Surf.Token.Output
{
    public class SurfTokenOutput
    {
        [JsonProperty("access_token")]
        public string Token { get; set; }
        [JsonProperty("token_type")]
        public string Type { get; set; }
        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
    }
}
