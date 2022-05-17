using Business.API.Hub.Integration.Surf.Portability;
using DAO.DBConnection;
using DTO.Hub.Cellphone.Input;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace XApi.Controllers.Hub.Surf
{
    [ApiController]
    public class PortabilityController : SurfBaseController<PortabilityController>
    {
        protected BlSurfPortability Bl;
        public PortabilityController(ILogger<PortabilityController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);

        [HttpPost, Route("resend-sms")]
        public async Task<IActionResult> ResendSms(HubCellphonePortabilityInput input) => Ok(await Bl.ResendSms(input).ConfigureAwait(false));

        [HttpPost, Route("check-status")]
        public async Task<IActionResult> CheckStatus(HubCellphonePortabilityInput input) => Ok(await Bl.CheckStatus(input).ConfigureAwait(false));
    }
}
