using Business.API.Hub.Permission;
using DAO.DBConnection;
using DTO.Hub.Permission.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using XApi.Controllers.Hub.Application;

namespace XApi.Controllers.Hub.Permission
{
    [ApiController]
    public class AllyPermissionController : PermissionBaseController<AllyPermissionController>
    {
        protected BlAllyPermission Bl;
        public AllyPermissionController(ILogger<AllyPermissionController> logger, XDataDatabaseSettings settings) : base(logger)
        {
            Bl = new(settings);
        }

        [HttpGet, Route("get-permissions")]
        public IActionResult GetAllyPermission(string allyId) => Ok(Bl.GetAllyPermission(allyId));

        [HttpPost, Route("upsert-product-permission")]
        public IActionResult UpsertPermissions(HubAllyPermission permission) => Ok(Bl.UpsertAllyPermission(permission));
    }
}
