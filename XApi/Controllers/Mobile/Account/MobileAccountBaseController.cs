using DTO.API.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Mobile.Account
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Conta Mobile"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Mobile/Account/[controller]")]
    public class MobileAccountBaseController<T> : BaseController<T>
    {
        public MobileAccountBaseController(ILogger<T> logger) : base(logger) { }
    }
}
