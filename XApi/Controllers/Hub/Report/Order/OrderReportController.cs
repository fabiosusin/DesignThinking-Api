using Business.API.Hub.Report.Order;
using DAO.DBConnection;
using DTO.Hub.Order.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Hub.Report.Order
{
    [ApiController]
    public class OrderReportController : ReportBaseController<OrderReportController>
    {
        protected BlOrderReport Bl;
        public OrderReportController(ILogger<OrderReportController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new BlOrderReport(settings);

        [HttpPost, Route("list"), AllowAnonymous]
        public IActionResult List(HubOrderListInput input) => Ok(Bl.List(input));
    }
}
