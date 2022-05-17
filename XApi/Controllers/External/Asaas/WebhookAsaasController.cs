using Business.API.External.Asaas;
using DAO.DBConnection;
using DTO.External.Asaas.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.External.Asaas
{
    [ApiController, AllowAnonymous]
    public class WebhookAsaasController : AsaasBaseController<WebhookAsaasController>
    {
        protected BlWebhookAsaas Bl;
        public WebhookAsaasController(ILogger<WebhookAsaasController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);

        [HttpPost, Route("update-charge")]
        public IActionResult UpdateCharge(AsaasUpdateInput input)
        {
            Bl.UpdateChargeAsync(input);
            return Ok();
        }
    }
}
