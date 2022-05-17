using Newtonsoft.Json;

namespace DTO.Integration.Surf.Token.Input
{
    public class SurfDetailsGetTokenInput
    {
        public string Login { get; set; }

        [JsonProperty("senha")]
        public string Password { get; set; }
        public string GrantType { get; set; }
    }
}
