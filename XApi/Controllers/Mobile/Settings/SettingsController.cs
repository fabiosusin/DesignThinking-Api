using Business.API.General.BlAppSettings;
using DAO.DBConnection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Mobile.Settings
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Configurações"), AllowAnonymous]
    [Route("v1/Mobile/Settings")]
    public class SettingsController : BaseController<SettingsController>
    {
        protected BlAppSettings Bl;
        public SettingsController(ILogger<SettingsController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new BlAppSettings(settings);

        [HttpGet, Route("get-app-settings")]
        public IActionResult GetAppSettings(string allyId) => Ok(Bl.GetAppSettings(allyId));
    }
}
