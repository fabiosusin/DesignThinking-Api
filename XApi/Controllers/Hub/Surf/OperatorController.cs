using Business.API.Hub.Integration.Surf.Operator;
using DAO.DBConnection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace XApi.Controllers.Hub.Surf
{
    [ApiController]
    public class OperatorController : SurfBaseController<OperatorController>
    {
        protected BlSurfOperator Bl;
        public OperatorController(ILogger<OperatorController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);

        [HttpGet, Route("get-operators")]
        public async Task<IActionResult> GetOperators() => Ok(await Bl.GetOperators().ConfigureAwait(false));
    }
}
