using DTO.Surf.Enum;
using Newtonsoft.Json;

namespace DTO.Integration.Surf.BaseDetails.Output
{
    public class SurfDetailsBaseApiOutput
    {
        [JsonProperty("Code")]
        public string CodeStr { get; set; }
        [JsonIgnore]
        public AppReturnCodesEnum Code { get => AppReturnCodesEnum.Unknown.GetByDescriptionDtoConverter(CodeStr); set { } }
        public string Msg { get; set; }
    }
}
