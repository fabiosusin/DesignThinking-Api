using Business.API.General.Faq;
using Business.API.Hub.Application.Faq;
using DAO.DBConnection;
using DTO.General.Faq.Input;
using DTO.Hub.Application.Faq.Input;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Hub.Application
{
    [ApiController]
    public class FaqsController : ApplicationBaseController<FaqsController>
    {
        private readonly BlFaq BlFaq;
        private readonly BlHubFaq BlHubFaq;

        public FaqsController(ILogger<FaqsController> logger, XDataDatabaseSettings settings) : base(logger)
        {
            BlFaq = new(settings);
            BlHubFaq = new(settings);
        }

        [HttpPost, Route("list-app-faq")]
        public IActionResult ListFaq(FaqListInput input) => Ok(BlFaq.ListFaq(input));

        [HttpPost, Route("upsert-app-faq")]
        public IActionResult UpsertAppFaq(HubAppFaqInput input) => Ok(BlHubFaq.UpsertAppFaq(input));

        [HttpPost, Route("remove-app-faq")]
        public IActionResult RemoveAppFaq(HubAppFaqInput input) => Ok(BlHubFaq.RemoveAppFaq(input));
    }
}
