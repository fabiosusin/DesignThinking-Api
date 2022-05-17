using Business.API.Mobile.Account;
using DAO.DBConnection;
using DTO.General.Image.Input;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace XApi.Controllers.Mobile.Account
{
    [ApiController]
    public class HomeController : MobileAccountBaseController<HomeController>
    {
        protected BlHome Bl;
        public HomeController(ILogger<HomeController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new BlHome(settings);

        [HttpGet, Route("get-registered-cellphones")]
        public async Task<IActionResult> GetRegisteredCellPhones(string mobileId, string allyId) => Ok(await Bl.GetRegisteredCellPhones(mobileId, allyId).ConfigureAwait(false));

        [HttpGet, Route("get-sponsors")]
        public IActionResult GetSponsors(string allyId, ListResolutionsSize imgSize) => Ok(Bl.GetHomeSponsorsOutput(allyId, imgSize));
    }
}
