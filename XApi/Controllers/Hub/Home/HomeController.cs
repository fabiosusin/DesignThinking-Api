using Business.API.Hub.Home;
using DAO.DBConnection;
using DTO.API.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Hub.Home
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Home Hub"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Hub/[controller]")]
    public class HomeController : BaseController<HomeController>
    {
        public HomeController(ILogger<HomeController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);
        protected BlHubHome Bl;

        [HttpGet, Route("get-data")]
        public IActionResult GetData(string allyId) => Ok(Bl.GetHomeData(allyId));
    }
}
