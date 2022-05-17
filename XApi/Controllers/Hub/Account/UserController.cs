using Business.API.Hub.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DAO.DBConnection;
using DTO.Hub.User.Input;
using System.Threading.Tasks;
using DTO.API.Auth;
using Microsoft.AspNetCore.Authorization;

namespace XApi.Controllers.Hub.Account
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Conta Hub"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Hub/User")]
    public class UserController : BaseController<UserController>
    {
        public UserController(ILogger<UserController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);
        protected BlHubAuth Bl;

        [HttpPost, Route("upsert-user")]
        public IActionResult UpsertAccount(HubAddUserInput input) => Ok(Bl.UpsertUser(input));

        [HttpGet, Route("get-user-by-email")]
        public IActionResult GetAccountByEmail(string email) => Ok(Bl.FindAccountByEmail(email));

        [HttpPost, Route("send-temp-password/{userEmail}"), AllowAnonymous]
        public async Task<IActionResult> SendTempPassword(string userEmail) => Ok(await Bl.SendTempPassword(userEmail).ConfigureAwait(false));

        [HttpPost, Route("update-user")]
        public IActionResult UpdateUser(HubUpdateUserInput input) => Ok(Bl.UpdateUser(input));

        [HttpDelete, Route("delete/{id}")]
        public IActionResult DeleteUser(string id) => Ok(Bl.DeleteUser(id));

        [HttpPost, Route("list")]
        public IActionResult ListUser(HubUserListInput input) => Ok(Bl.List(input));
    }
}
