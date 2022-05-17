using Business.API.Mobile.Surf;
using DAO.DBConnection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace XApi.Controllers.Mobile.Surf
{

    [ApiController]
    public class AccountDetailsController : SurfBaseController<AccountDetailsController>
    {
        protected BlAccountDetails Bl;
        public AccountDetailsController(ILogger<AccountDetailsController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new BlAccountDetails(settings);

        [HttpGet, Route("get-details")]
        public async Task<IActionResult> GetDetails(string number, string countryPrefix, string allyId) => Ok(await Bl.GetDetailsByMsisdn(number, countryPrefix, allyId).ConfigureAwait(false));

        [HttpGet, Route("get-details-cpf")]
        public async Task<IActionResult> GetDetailsCpf(string cpf) => Ok(await Bl.GetDetailsByCpf(cpf).ConfigureAwait(false));

        [HttpPost, Route("upsert-customer-msisdn")]
        public IActionResult UpsertCustomerMsisdn(string number, string countryPrefix, string mobileId, string allyId, bool insert) => Ok(Bl.UpsertCustomerMsisdn(number, countryPrefix, mobileId, allyId, insert));
    }
}
