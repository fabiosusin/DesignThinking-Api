using DTO.API.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Hub.Application
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Permissões"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Hub/Permission/[controller]")]
    public class PermissionBaseController<T> : BaseController<T>
    {
        public PermissionBaseController(ILogger<T> logger) : base(logger) { }
    }
}
