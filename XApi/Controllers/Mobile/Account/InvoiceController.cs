using Business.API.Mobile.Account;
using DAO.DBConnection;
using DTO.Mobile.Account.Input;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace XApi.Controllers.Mobile.Account
{
    [ApiController]
    public class InvoiceController : MobileAccountBaseController<InvoiceController>
    {
        protected BlInvoiceCustomer Bl;

        public InvoiceController(ILogger<InvoiceController> logger, XDataDatabaseSettings settings) : base(logger)
        {
            Bl = new BlInvoiceCustomer(settings);
        }

        [HttpGet, Route("get-invoice-details")]
        public async Task<IActionResult> GetInvoiceDetails(string invoiceId, string allyId) => Ok(await Bl.GetInvoiceDetails(invoiceId, allyId).ConfigureAwait(false));

        [HttpGet, Route("get-invoice-unpaid-current-month")]
        public async Task<IActionResult> GetInvoiceCurrentMonth(string number, string mobileId, string allyId) => Ok(await Bl.GetInvoiceUnpaid(number, mobileId, allyId).ConfigureAwait(false));

        [HttpGet, Route("get-invoice-expired")]
        public async Task<IActionResult> GetInvoiceExpired(string number, string mobileId, string allyId) => Ok(await Bl.GetInvoiceUnpaid(number, mobileId, allyId, false).ConfigureAwait(false));

        [HttpPost, Route("List")]
        public IActionResult List(AppInvoiceListInput input) => Ok(Bl.List(input));
    }
}
