using DTO.Integration.Surf.BaseDetails.Output;
using DTO.Surf.Enum;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DTO.Integration.Surf.Call.Output
{
    public class SurfCallHistoryOutput : SurfDetailsBaseApiOutput
    {
        public string Msisdn { get; set; }
        public List<AppCallDetail> Details { get; set; }
    }

    public class AppCallDetail
    {
        [JsonProperty("CallType")]
        public string CallTypeStr { get; set; }
        [JsonIgnore]
        public AppCallDataTypeEnum CallType { get => AppCallDataTypeEnum.Unknown.GetByDescriptionDtoConverter(CallTypeStr); set { } }
        public string CalledNumber { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Duration { get; set; }
        public string Cost { get; set; }
        public object BundleName { get; set; }
        public string BundleCode { get; set; }
        public string InitialFreeUnits { get; set; }
        public string FinalFreeUnits { get; set; }

        [JsonProperty("totalUsed Units")]
        public string TotalUsedUnits { get; set; }
        public string TotalUsedBytes { get; set; }
        public string RemainaingBalance { get; set; }
    }
}
