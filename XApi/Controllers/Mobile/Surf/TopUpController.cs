using Business.API.Mobile.Surf;
using DAO.DBConnection;
using DTO.Integration.Surf.TopUp.Input;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace XApi.Controllers.Mobile.Surf
{

    [ApiController]
    public class TopUpController : SurfBaseController<TopUpController>
    {
        protected BlTopUp Bl;
        public TopUpController(ILogger<TopUpController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new BlTopUp(settings);

        [HttpPost, Route("top-up-history")]
        public async Task<IActionResult> TopUpHistory([FromBody] SurfTopUpHistoryInput input) => Ok(await Bl.TopUpHistory(input).ConfigureAwait(false));
    }
}
