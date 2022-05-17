using DTO.API.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.External.Asaas
{

    [ApiController, Route("v1/External/Asaas/[controller]"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [ApiExplorerSettings(GroupName = "Asaas - Externo")]
    public class AsaasBaseController<T> : BaseController<T>
    {
        public AsaasBaseController(ILogger<T> logger) : base(logger) { }
    }
}
