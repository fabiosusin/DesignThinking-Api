using DTO.General.Base.Api.Output;
using DTO.Hub.Order.Enum;
using DTO.Hub.Order.Output;
using System.Collections.Generic;

namespace DTO.Hub.Report.Order.Output
{
    public class HubOrderReportListOutput : BaseApiOutput
    {
        public HubOrderReportListOutput(string msg) : base(msg) { }
        public HubOrderReportListOutput(IEnumerable<HubOrderListData> allys, HubOrderReportTotals totals) : base(true)
        {
            Orders = allys;
            Totals = totals;
        }

        public IEnumerable<HubOrderListData> Orders { get; set; }
        public HubOrderReportTotals Totals { get; set; }
    }

    public class HubOrderReportTotals
    {
        public HubOrderReportTotals() => PaymentsTotals = new List<HubOrderReportPaymentsTotals>();
        public decimal AllyTotal { get; set; }
        public decimal XPlayTotal { get; set; }
        public List<HubOrderReportPaymentsTotals> PaymentsTotals { get; set; }
    }

    public class HubOrderReportPaymentsTotals
    {
        public HubOrderReportPaymentsTotals(HubOrderPaymentFormEnum type) => Type = type;

        public HubOrderPaymentFormEnum Type { get; set; }
        public decimal Value { get; set; }
    }
}
