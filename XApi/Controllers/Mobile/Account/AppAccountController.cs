using Business.API.Mobile.Account;
using DAO.DBConnection;
using DTO.Mobile.Account.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace XApi.Controllers.Mobile.Account
{
    [ApiController]
    public class AppAccountController : MobileAccountBaseController<AppAccountController>
    {
        public AppAccountController(ILogger<AppAccountController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);

        protected BlAppAccount Bl;

        [HttpPost, Route("add-account"), AllowAnonymous]
        public IActionResult AddAccount(AppAddAccountInput input) => Ok(Bl.AddAccount(input));

        [HttpPost, Route("get-account-by-mobile-id"), AllowAnonymous]
        public IActionResult GetAccountByMobileId(AppLoginInput input) => Ok(Bl.FindAccountByMobileId(input));

        [HttpPost, Route("update-account")]
        public IActionResult UpdateAccount(AppUpdateAccountDataInput input) => Ok(Bl.UpdateAccount(input));
    }
}
