using DTO.API.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.General.Files
{
    [ApiController, Route("v1/General/Files/[controller]"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [ApiExplorerSettings(GroupName = "Arquivos - Externo")]
    public class FilesBaseController<T> : BaseController<T>
    {
        public FilesBaseController(ILogger<T> logger) : base(logger) { }
    }
}
