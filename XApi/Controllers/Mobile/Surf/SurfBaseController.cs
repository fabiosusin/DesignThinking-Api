using DTO.API.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Mobile.Surf
{
    [ApiController, Route("v1/Mobile/Surf/[controller]"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [ApiExplorerSettings(GroupName = "Surf")]
    public class SurfBaseController<T> : BaseController<T>
    {
        public SurfBaseController(ILogger<T> logger) : base(logger) { }
    }
}
