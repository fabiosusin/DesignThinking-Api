using Business.API.Hub.Customer;
using DAO.DBConnection;
using DTO.API.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace XApi.Controllers.Hub.Customer
{
    [ApiController]
    public class InvoiceController : CustomerBaseController<InvoiceController>
    {
        protected BlInvoiceCustomer Bl;

        public InvoiceController(ILogger<InvoiceController> logger, XDataDatabaseSettings settings) : base(logger)
        {
            Bl = new BlInvoiceCustomer(settings);
        }

        [HttpGet, Route("invoice-details")]
        public async Task<IActionResult> GetInvoiceDetails(string invoiceId) => Ok(await Bl.GetChargeDetails(invoiceId).ConfigureAwait(false));

        [HttpPost, Route("paid-invoice")]
        public async Task<IActionResult> PaidInvoice(string invoiceId, string userId) => Ok(await Bl.PaidInvoice(invoiceId, userId).ConfigureAwait(false));
    }
}
