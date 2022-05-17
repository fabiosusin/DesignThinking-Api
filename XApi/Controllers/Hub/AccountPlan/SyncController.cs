using Business.API.Hub.AccountPlan;
using DAO.DBConnection;
using DTO.Integration.Sige.AccountPlan.Input;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace XApi.Controllers.Hub.AccountPlan
{
    [ApiController]
    public class SyncController : AccountPlanBaseController<SyncController>
    {
        public SyncController(ILogger<SyncController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);
        protected BlSyncAccountPlan Bl;

        [HttpPost, Route("sync")]
        public async Task<IActionResult> Sync(SigeAccountPlanFiltersInput input) => Ok(await Bl.SyncAccountPlan(input).ConfigureAwait(false));
    }
}
