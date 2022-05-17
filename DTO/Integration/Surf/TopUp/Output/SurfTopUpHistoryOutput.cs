using DTO.Integration.Surf.BaseDetails.Output;
using System.Collections.Generic;

namespace DTO.Integration.Surf.TopUp.Output
{
    public class SurfTopUpHistoryOutput : SurfDetailsBaseApiOutput
    {
        public string MSISDN { get; set; }
        public List<TopUpDetail> Details { get; set; }
    }

    public class TopUpDetail
    {
        public string Type { get; set; }
        public string Date { get; set; }
        public string Amount { get; set; }
        public string Bonus { get; set; }
        public string Time { get; set; }
    }
}
