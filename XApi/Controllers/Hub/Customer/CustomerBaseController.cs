using DTO.API.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Hub.Customer
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Cliente"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Hub/Customer/[controller]")]
    public class CustomerBaseController<T> : BaseController<T>
    {
        public CustomerBaseController(ILogger<T> logger) : base(logger) { }
    }
}
