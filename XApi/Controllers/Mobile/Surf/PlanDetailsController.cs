using Business.API.Mobile.Surf;
using Microsoft.AspNetCore.Mvc;
using DAO.DBConnection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace XApi.Controllers.Mobile.Surf
{

    [ApiController]
    public class PlanDetailsController : SurfBaseController<PlanDetailsController>
    {
        protected BlPlanDetails Bl;

        public PlanDetailsController(ILogger<PlanDetailsController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new BlPlanDetails(settings);

        [HttpGet, Route("bundle-info")]
        public async Task<IActionResult> BundleInfo(string msisdn) => Ok(await Bl.BundleInfo(msisdn).ConfigureAwait(false));

        [HttpPost, Route("free-usage-info")]
        public async Task<IActionResult> FreeUsageInfo(string msisdn, string bundleCode) => Ok(await Bl.FreeUsageInfo(msisdn, bundleCode).ConfigureAwait(false));

        [HttpGet, Route("get-account-plan-info")]
        public async Task<IActionResult> GetAccountPlanInfo(string msisdn) => Ok(await Bl.GetAccountPlanInfo(msisdn).ConfigureAwait(false));

    }
}
