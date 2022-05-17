using Business.API.Hub.Integration.Surf.CustomerMsisdn;
using DAO.DBConnection;
using DTO.General.Surf.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Hub.Surf
{
    [ApiController]
    public class CustomerMsisdnController : SurfBaseController<CustomerMsisdnController>
    {
        protected BlCustomerMsisdn Bl;
        public CustomerMsisdnController(ILogger<CustomerMsisdnController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new BlCustomerMsisdn(settings);

        [HttpPost, Route("create-msisdn")]
        public IActionResult CreateMsisdn(SurfCustomerMsisdnInput input) => Ok(Bl.InsertCustomerMsisdn(input));

        [HttpDelete, Route("remove-msisdn/{countryPrefix}/{number}")]
        public IActionResult RemoveMsisdn(string countryPrefix, string number) => Ok(Bl.RemoveCustomerMsisdn(countryPrefix, number));

    }
}
