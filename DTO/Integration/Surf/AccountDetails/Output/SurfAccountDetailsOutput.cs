using DTO.Integration.Surf.BaseDetails.Output;

namespace DTO.Integration.Surf.AccountDetails.Output
{
    public class SurfAccountDetailsOutput : SurfDetailsBaseApiOutput
    {
        public string Description { get; set; }
        public string Msisdn { get; set; }
        public string NetworkID { get; set; }
        public string AccountStatus { get; set; }
        public string ValidityDate { get; set; }
        public string CurrBalance { get; set; }
        public string LanguageID { get; set; }
        public string PlanID { get; set; }
        public string SimStatus { get; set; }
        public string ICCID { get; set; }
    }
}
