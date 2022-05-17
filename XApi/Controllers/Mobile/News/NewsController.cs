using Business.API.General.Wix;
using Business.API.Mobile.News;
using DAO.DBConnection;
using DTO.API.Auth;
using DTO.External.Wix.Input;
using DTO.Mobile.News.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Mobile.News
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Notícias"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Mobile/News")]
    public class NewsController : BaseController<NewsController>
    {
        protected BlNews Bl;
        protected BlWixCategory BlWixCategory;
        protected BlWixTag BlWixTag;
        public NewsController(ILogger<NewsController> logger, XDataDatabaseSettings settings) : base(logger)
        {
            Bl = new(settings);
            BlWixTag = new(settings);
            BlWixCategory = new(settings);
        }

        [HttpGet, Route("get-news-details")]
        public IActionResult GetNewsDetails(string id) => Ok(Bl.GetNewsDetails(id));

        [HttpPost, Route("get-news")]
        public IActionResult GetNews(AppNewsListInput input) => Ok(Bl.GetNewsList(input));

        [HttpGet, Route("get-category-by-wix-id")]
        public IActionResult GetCategory(string id) => Ok(BlWixCategory.GetCategoryDetailsByWixId(id));

        [HttpPost, Route("list-categories")]
        public IActionResult ListCategories(WixCategoryListInput input) => Ok(BlWixCategory.List(input));

        [HttpPost, Route("list-tags")]
        public IActionResult ListTags(WixTagListInput input) => Ok(BlWixTag.List(input));

    }
}
