using DTO.API.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Hub.AccountPlan
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Plano de Conta Hub"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Hub/AccountPlan/[controller]")]
    public class AccountPlanBaseController<T> : BaseController<T>
    {
        public AccountPlanBaseController(ILogger<T> logger) : base(logger) { }
    }
}
