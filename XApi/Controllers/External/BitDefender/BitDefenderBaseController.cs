using DTO.API.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.External.BitDefender
{
    [ApiController, Route("v1/External/BitDefender/[controller]"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [ApiExplorerSettings(GroupName = "BitDefender - Externo")]
    public class BitDefenderBaseController<T> : BaseController<T>
    {
        public BitDefenderBaseController(ILogger<T> logger) : base(logger) { }
    }
}
