using Business.API.Mobile.Account;
using DAO.DBConnection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Mobile.Account
{
    [ApiController]
    public class SubscriberAreaController : MobileAccountBaseController<SubscriberAreaController>
    {
        protected BlSubscriberArea Bl;

        public SubscriberAreaController(ILogger<SubscriberAreaController> logger, XDataDatabaseSettings settings) : base(logger)
        {
            Bl = new BlSubscriberArea(settings);
        }

        [HttpGet, Route("get-services")]
        public IActionResult GetServices(string mobileId, string allyId) => Ok(Bl.GetServices(mobileId, allyId));

    }
}
