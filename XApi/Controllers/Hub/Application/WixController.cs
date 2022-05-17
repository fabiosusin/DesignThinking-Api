using Business.API.General.Wix;
using Business.API.Hub.Application.Wix;
using DAO.DBConnection;
using DTO.External.Wix.Input;
using DTO.Hub.Application.Wix.Input;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Hub.Application
{
    [ApiController]
    public class WixController : ApplicationBaseController<WixController>
    {
        protected BlHubWixCategory BlHubWixCategory;
        protected BlHubWixTag BlHubWixTag;
        protected BlWixCategory BlWixCategory;
        protected BlWixTag BlWixTag;
        public WixController(ILogger<WixController> logger, XDataDatabaseSettings settings) : base(logger)
        {
            BlWixTag = new(settings);
            BlHubWixTag = new(settings);
            BlWixCategory = new(settings);
            BlHubWixCategory = new(settings);
        }

        [HttpPost, Route("list-categories")]
        public IActionResult ListCategories(WixCategoryListInput input) => Ok(BlWixCategory.List(input));

        [HttpPost, Route("upsert-ally-category")]
        public IActionResult UpsertAllyCategory(HubWixAllyCategoryInput input) => Ok(BlHubWixCategory.UpsertAllyCategory(input));

        [HttpPost, Route("list-tags")]
        public IActionResult ListTags(WixTagListInput input) => Ok(BlWixTag.List(input));

        [HttpPost, Route("upsert-ally-tag")]
        public IActionResult UpsertAllyTag(HubWixAllyTagInput input) => Ok(BlHubWixTag.UpsertAllyTag(input));

    }
}
