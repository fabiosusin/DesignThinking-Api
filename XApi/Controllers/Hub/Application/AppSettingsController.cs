using Business.API.General.BlAppSettings;
using Business.API.Hub.Application.Settings;
using DAO.DBConnection;
using DTO.Hub.Application.AppSettings.Input;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Hub.Application
{

    [ApiController]
    public class AppSettingsController : ApplicationBaseController<AppSettingsController>
    {
        protected BlAppSettings BlAppSettings;
        protected BlHubAppSettings BlHubAppSettings;
        public AppSettingsController(ILogger<AppSettingsController> logger, XDataDatabaseSettings settings) : base(logger)
        {
            BlHubAppSettings = new(settings);
            BlAppSettings = new(settings);
        }

        [HttpGet, Route("get-app-settings")]
        public IActionResult GetAppSettings(string allyId) => Ok(BlAppSettings.GetAppSettings(allyId));

        [HttpPost, Route("upsert-app-settings")]
        public IActionResult UpsertAppSettings(HubAppSettingsInput input) => Ok(BlHubAppSettings.UpsertAppSettings(input));
    }
}
