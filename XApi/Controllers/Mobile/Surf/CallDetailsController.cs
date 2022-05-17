using Business.API.Mobile.Surf;
using DAO.DBConnection;
using DTO.Integration.Surf.Call.Input;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace XApi.Controllers.Mobile.Surf
{

    [ApiController]
    public class CallDetailsController : SurfBaseController<CallDetailsController>
    {
        protected BlCallDetails Bl;
        public CallDetailsController(ILogger<CallDetailsController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new BlCallDetails(settings);

        [HttpPost, Route("call-history")]
        public async Task<IActionResult> CallHistory([FromBody] SurfCallHistoryInput input) => Ok(await Bl.CallHistory(input).ConfigureAwait(false));

        [HttpGet, Route("call-history-chart-current-month")]
        public async Task<IActionResult> CallHistoryChartCurrentMoth(string msisdn) => Ok(await Bl.CallHistoryChartCurrentMonth(msisdn).ConfigureAwait(false));
    }
}
