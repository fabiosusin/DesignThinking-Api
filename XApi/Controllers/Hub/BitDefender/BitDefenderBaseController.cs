using DTO.API.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Hub.BitDefender
{
    [ApiController, Route("v1/Hub/BitDefender/[controller]"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [ApiExplorerSettings(GroupName = "BitDefender")]
    public class BitDefenderBaseController<T> : BaseController<T>
    {
        public BitDefenderBaseController(ILogger<T> logger) : base(logger) { }
    }
}
