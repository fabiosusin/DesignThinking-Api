using DTO.API.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Hub.Surf
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Surf - Hub"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Hub/Surf/[controller]")]
    public class SurfBaseController<T> : BaseController<T>
    {
        public SurfBaseController(ILogger<T> logger) : base(logger) { }
    }
}
