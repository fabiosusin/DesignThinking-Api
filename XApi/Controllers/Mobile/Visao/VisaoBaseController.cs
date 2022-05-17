using DTO.API.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Mobile.Visao
{
    [ApiController, Route("v1/Mobile/Visao/[controller]"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [ApiExplorerSettings(GroupName = "Visão")]
    public class VisaoBaseController<T> : BaseController<T>
    {
        public VisaoBaseController(ILogger<T> logger) : base(logger) { }
    }
}
