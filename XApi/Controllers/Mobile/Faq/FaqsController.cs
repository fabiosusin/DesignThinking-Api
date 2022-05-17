using Business.API.General.Faq;
using DAO.DBConnection;
using DTO.API.Auth;
using DTO.General.Faq.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Mobile.Faq
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Faq"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Mobile/Faq")]
    public class FaqsController : BaseController<FaqsController>
    {
        private readonly BlFaq BlFaq;

        public FaqsController(ILogger<FaqsController> logger, XDataDatabaseSettings settings) : base(logger)
        {
            BlFaq = new(settings);
        }

        [HttpPost, Route("list-app-faq")]
        public IActionResult ListFaq(FaqListInput input) => Ok(BlFaq.ListFaq(input));
    }
}
