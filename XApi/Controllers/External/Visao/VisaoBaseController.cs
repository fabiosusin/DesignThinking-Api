using DTO.API.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.External.Visao
{
    [ApiController, Route("v1/External/Visao/[controller]"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [ApiExplorerSettings(GroupName = "Visão - Externo")]
    public class VisaoBaseController<T> : BaseController<T>
    {
        public VisaoBaseController(ILogger<T> logger) : base(logger) { }
    }
}
