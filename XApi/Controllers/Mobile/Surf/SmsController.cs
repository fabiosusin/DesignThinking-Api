using Business.API.Mobile.Surf;
using DAO.DBConnection;
using DTO.Integration.Surf.SMS.Input;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace XApi.Controllers.Mobile.Surf
{
    // TODO: Será removido quando a versão superior a atual (1.3.0) do app para Android e IOS for publicada
    [ApiController]
    public class SmsController : SurfBaseController<SmsController>
    {
        protected BlSurfSms Bl;

        public SmsController(ILogger<SmsController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new BlSurfSms(settings);

        [HttpPost, Route("send-sms")]
        public async Task<IActionResult> SendSms(SurfSendSmsOtpInput input) => Ok(await Bl.SendSms(input).ConfigureAwait(false));

        [HttpPost, Route("send-sms-otp")]
        public async Task<IActionResult> SendSmsOtp(SurfSendSmsOtpInput input) => Ok(await Bl.SendSms(input).ConfigureAwait(false));

        [HttpPost, Route("validate-sms-otp")]
        public async Task<IActionResult> ValidateSmsOtp(SurfValidateSmsOtpInput input) => Ok(await Bl.ValidateSmsOpt(input).ConfigureAwait(false));
    }
}
