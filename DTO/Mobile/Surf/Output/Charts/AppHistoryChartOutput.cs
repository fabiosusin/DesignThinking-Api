using DTO.Integration.Surf.BaseDetails.Output;
using System.Collections.Generic;

namespace DTO.Surf.Output.Charts
{
    public class AppHistoryChartOutput : SurfDetailsBaseApiOutput
    {
        public List<BaseHistoryChart> Internet { get; set; } = new List<BaseHistoryChart>();
        public List<BaseHistoryChart> Sms { get; set; } = new List<BaseHistoryChart>();
        public List<BaseHistoryChart> Call { get; set; } = new List<BaseHistoryChart>();
    }

    public class BaseHistoryChart
    {
        public double Data { get; set; }
        public string MinDate { get; set; }
        public string FullDate { get; set; }
    }


}
