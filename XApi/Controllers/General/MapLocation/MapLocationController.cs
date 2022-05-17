using Business.API.General.MapLocation;
using DAO.DBConnection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace XApi.Controllers.General.MapLocation
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Geral"), AllowAnonymous]
    [Route("v1/General/MapLocation")]
    public class MapLocationController : BaseController<MapLocationController>
    {
        public MapLocationController(ILogger<MapLocationController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);
        protected BlGeneral Bl;

        [HttpGet, Route("get-address")]
        public async Task<IActionResult> GetAddressAsync(string zipCode) => Ok(await Bl.GetAddressAsync(zipCode).ConfigureAwait(false));
    }
}
