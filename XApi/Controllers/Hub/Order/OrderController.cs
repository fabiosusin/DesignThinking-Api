using Business.API.Hub.Order;
using DAO.DBConnection;
using DTO.API.Auth;
using DTO.Hub.Order.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace XApi.Controllers.Hub.Order
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Venda"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Hub/[controller]")]
    public class OrderController : BaseController<OrderController>
    {
        public OrderController(ILogger<OrderController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);
        private readonly BlOrder Bl;

        [HttpPost, Route("create-order")]
        public async Task<IActionResult> CreateOrder(HubOrderCreationInput input) => Ok(await Bl.CreateOrder(input).ConfigureAwait(false));

        [HttpPost, Route("reprocess-order-payment/{paymentOrderId}")]
        public async Task<IActionResult> ReprocessOrderCharge(string paymentOrderId, HubOrderInputPaymentData payment) => Ok(await Bl.ReprocessOrderCharge(paymentOrderId, payment).ConfigureAwait(false));

        [HttpGet, Route("get-order/{orderId}")]
        public async Task<IActionResult> GetOrderOutputAsync(string orderId) => Ok(await Bl.GetOrderDetails(orderId).ConfigureAwait(false));

        [HttpPost, Route("generate-order-nfse")]
        public async Task<IActionResult> GenerateOrderNfse(string orderId) => Ok(await Bl.GenerateOrderNfse(orderId).ConfigureAwait(false));

        [HttpPost, Route("get-order-nfse-status")]
        public async Task<IActionResult> GetOrderNfseStatus(string orderId) => Ok(await Bl.GetOrderNfseStatus(orderId).ConfigureAwait(false));

        [HttpPost, Route("get-order-nfse-details")]
        public async Task<IActionResult> GetOrderNfseDetails(string orderId) => Ok(await Bl.GetOrderNfseDetails(orderId).ConfigureAwait(false));

        [HttpPost, Route("list")]
        public IActionResult ListUser(HubOrderListInput input) => Ok(Bl.List(input));
    }
}
