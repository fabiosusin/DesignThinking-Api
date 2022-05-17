using Newtonsoft.Json;

namespace DTO.Integration.Surf.Token.Input
{
    public class SurfGenerateTokenInput
    {
        public SurfGenerateTokenInput(string type) => GrantType = type;
        [JsonProperty("grant_type")]
        public string GrantType { get; set; }
    }
}
