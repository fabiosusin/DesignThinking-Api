using Business.API.Hub.BitDefender;
using DAO.DBConnection;
using DTO.Hub.BitDefender.Input;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Hub.BitDefender
{
    [ApiController]
    public class LicenseController : BitDefenderBaseController<LicenseController>
    {
        public LicenseController(ILogger<LicenseController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);

        protected BlLicense Bl;

        [HttpPost, Route("list")]
        public IActionResult List(HubBitDefenderLicensesListInput input) => Ok(Bl.List(input));

        [HttpGet, Route("categories")]
        public IActionResult Categories() => Ok(Bl.GetCategories());
    }
}
