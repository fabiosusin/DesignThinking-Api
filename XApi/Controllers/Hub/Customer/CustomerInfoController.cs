using Business.API.Hub.BlCustomer;
using DAO.DBConnection;
using DTO.Hub.Customer.Database;
using DTO.Hub.Customer.Input;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Hub.Customer
{
    [ApiController]
    public class CustomerInfoController : CustomerBaseController<CustomerInfoController>
    {
        public CustomerInfoController(ILogger<CustomerInfoController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);
        protected BlCustomer Bl;

        [HttpPost, Route("upsert-customer")]
        public IActionResult UpsertCustomer(HubCustomer input) => Ok(Bl.UpsertCustomer(input));

        [HttpGet, Route("get-by-document/{cpfCnpj}/{allyId}")]
        public IActionResult GetCustomer(string cpfCnpj, string allyId) => Ok(Bl.GetCustomer(cpfCnpj, allyId));

        [HttpDelete, Route("delete/{id}/{allyId}")]
        public IActionResult DeleteCustomer(string id, string allyId) => Ok(Bl.DeleteCustomer(id, allyId));

        [HttpPost, Route("list")]
        public IActionResult ListCustomer(HubCustomerListInput input) => Ok(Bl.List(input));
    }
}
