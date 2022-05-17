using DTO.Integration.Surf.BaseDetails.Output;
using Newtonsoft.Json;

namespace DTO.Integration.Surf.AccountPlan.Output
{
    public class SurfBundleInfoOutput : SurfDetailsBaseApiOutput
    {
        public string Description { get; set; }
        public string Msisdn { get; set; }
        public string BundleCode { get; set; }
        public string BundleName { get; set; }
        public string BundleExpiry { get; set; }
        public string BundleStatus { get; set; }

        [JsonProperty("bundleStart Date")]
        public string BundleStartDate { get; set; }
    }
}
