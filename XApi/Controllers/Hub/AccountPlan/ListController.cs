using Business.API.Hub.AccountPlan;
using DAO.DBConnection;
using DTO.Hub.AccountPlan.Input;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Hub.AccountPlan
{
    [ApiController]
    public class ListController : AccountPlanBaseController<ListController>
    {
        public ListController(ILogger<ListController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);
        protected BlHubAccountPlan Bl;

        [HttpPost, Route("get-account-plans")]
        public IActionResult GetAccountPlans(HubAccountPlanListInput input) => Ok(Bl.List(input));
    }
}
