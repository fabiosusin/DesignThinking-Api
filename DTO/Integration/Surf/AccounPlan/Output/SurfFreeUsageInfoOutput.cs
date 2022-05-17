using DTO.Integration.Surf.BaseDetails.Output;

namespace DTO.Integration.Surf.AccountPlan.Output
{
    public class SurfFreeUsageInfoOutput : SurfDetailsBaseApiOutput
    {
        public string Description { get; set; }
        public string Msisdn { get; set; }
        public string BundleName { get; set; }
        public string BundleExpiry { get; set; }
        public string FreeNetMinutes { get; set; }
        public string FreeSMSMinutes { get; set; }
        public string FreeData { get; set; }
    }
}
