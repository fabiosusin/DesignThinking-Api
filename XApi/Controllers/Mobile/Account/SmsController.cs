using Business.API.Mobile.Account;
using DAO.DBConnection;
using DTO.Mobile.Account.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace XApi.Controllers.Mobile.Account
{
    [ApiController, AllowAnonymous]
    public class SmsController : MobileAccountBaseController<SmsController>
    {
        protected BlDefaultSms Bl;

        public SmsController(ILogger<SmsController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new BlDefaultSms(settings);

        [HttpPost, Route("send-sms")]
        public async Task<IActionResult> SendSms(AppSendSmsInput input) => Ok(await Bl.SendSms(input).ConfigureAwait(false));

        [HttpPost, Route("validate-sms-otp")]
        public async Task<IActionResult> ValidateSmsOtpAsync(AppValidateOtpInput input) => Ok(await Bl.ValidateSmsOtp(input).ConfigureAwait(false));

    }
}
