using DTO.API.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Hub.Application
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Aplicativos"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Hub/Application/[controller]")]
    public class ApplicationBaseController<T> : BaseController<T>
    {
        public ApplicationBaseController(ILogger<T> logger) : base(logger) { }
    }
}
