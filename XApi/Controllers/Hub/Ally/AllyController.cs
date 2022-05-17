using Business.API.Hub.BlAlly;
using DAO.DBConnection;
using DTO.API.Auth;
using DTO.Hub.Ally.Database;
using DTO.Hub.Ally.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace XApi.Controllers.Hub.AllyController
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Aliado"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Hub/[controller]")]
    public class AllyController : BaseController<AllyController>
    {
        public AllyController(ILogger<AllyController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);
        protected BlAlly Bl;

        [HttpGet, Route("get-ally")]
        public IActionResult GetAlly(string allyId) => Ok(Bl.GetAlly(allyId));

        [HttpPost, Route("upsert-ally")]
        public async Task<IActionResult> UpsertAlly(HubAlly input) => Ok(await Bl.UpsertAlly(input).ConfigureAwait(false));
        
        [HttpDelete, Route("delete/{id}")]
        public IActionResult DeleteAlly(string id) => Ok(Bl.DeleteAlly(id));

        [HttpPost, Route("list")]
        public IActionResult ListAlly(HubAllyListInput input) => Ok(Bl.List(input));
    }
}
