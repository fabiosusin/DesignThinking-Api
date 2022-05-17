using DAO.DBConnection;
using DAO.Hub.Order;
using DTO.Hub.Order.Input;
using DTO.Hub.Report.Order.Output;
using System.Linq;

namespace Business.API.Hub.Report.Order
{
    public class BlOrderReport
    {
        private readonly HubOrderDAO HubOrderDAO;

        public BlOrderReport(XDataDatabaseSettings settings)
        {
            HubOrderDAO = new(settings);
        }

        public HubOrderReportListOutput List(HubOrderListInput input)
        {
            var orders = HubOrderDAO.List(input);
            if (!(orders?.Any() ?? false))
                return new("Nenhuma Venda encontrada!");

            var totals = new HubOrderReportTotals();
            foreach (var order in orders)
            {
                totals.AllyTotal += order.Share?.Ally ?? 0;
                totals.XPlayTotal += order.Share?.XPlay ?? 0;

                foreach (var orderPayment in order.Payments)
                {
                    var payment = totals.PaymentsTotals.FirstOrDefault(x => x.Type == orderPayment.Type);
                    if (payment == null)
                    {
                        payment = new(orderPayment.Type);
                        totals.PaymentsTotals.Add(payment);
                    }

                    payment.Value += orderPayment.Value;
                }
            }

            return new(orders, totals);
        }
    }
}
