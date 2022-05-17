using DTO.API.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.External.Surf
{
    [ApiController, Route("v1/External/SurfBase/[controller]"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [ApiExplorerSettings(GroupName = "SurfBase - Externo")]
    public class SurfBaseController<T> : BaseController<T>
    {
        public SurfBaseController(ILogger<T> logger) : base(logger) { }
    }
}
