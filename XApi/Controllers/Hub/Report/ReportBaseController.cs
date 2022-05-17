using DTO.API.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Hub.Report
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Report"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Hub/Report/[controller]")]
    public class ReportBaseController<T> : BaseController<T>
    {
        public ReportBaseController(ILogger<T> logger) : base(logger) { }
    }
}
