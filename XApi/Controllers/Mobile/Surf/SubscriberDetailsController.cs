using Business.API.Mobile.Surf;
using DAO.DBConnection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace XApi.Controllers.Mobile.Surf
{

    [ApiController]
    public class SubscriberDetailsController : SurfBaseController<SubscriberDetailsController>
    {
        protected BlSubscriberDetails Bl;
        public SubscriberDetailsController(ILogger<SubscriberDetailsController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new BlSubscriberDetails(settings);

        [HttpGet, Route("subscriber-information")]
        public async Task<IActionResult> SubscriberInformation(string msisdn) => Ok(await Bl.SubscriberInformation(msisdn).ConfigureAwait(false));
    }
}
