using Business.API.Hub.Menu;
using DAO.DBConnection;
using DTO.API.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Hub.Menu
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Menu"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Hub/Menu")]
    public class MenuController : BaseController<MenuController>
    {
        protected BlHubMenu Bl;
        public MenuController(ILogger<MenuController> logger, XDataDatabaseSettings settings) : base(logger)
        {
            Bl = new(settings);
        }

        [HttpGet, Route("get-menu")]
        public IActionResult GetHubMenu(string allyId, string userId) => Ok(Bl.GetHubMenu(allyId, userId));

        [HttpGet, Route("get-menu-permission-options")]
        public IActionResult GetHubMenuPermissionOptions(string allyId, string userId) => Ok(Bl.GetHubMenuPermissionOptions(allyId, userId));
    }
}
