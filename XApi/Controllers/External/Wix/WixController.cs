using Business.API.External.Wix;
using DAO.DBConnection;
using DTO.External.Wix.Database;
using DTO.External.Wix.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XApi.Controllers.External.Wix
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Wix")]
    [Route("v1/External/Wix")]
    public class WixController : BaseController<WixController>
    {
        protected BlWix Bl;
        public WixController(ILogger<WixController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);

        [HttpPost, Route("register-tags")]
        public IActionResult RegisterTags(List<WixTagRegisterInput> tags) => Ok(Bl.RegisterTags(tags));

        [HttpPost, Route("register-posts")]
        public async Task<IActionResult> RegisterPosts(DateTime? date) => Ok(await Bl.RegisterPosts(date).ConfigureAwait(false));

        [HttpPost, Route("register-categories")]
        public async Task<IActionResult> RegisterCategories() => Ok(await Bl.RegisterCategories().ConfigureAwait(false));

        [HttpGet, Route("get-posts")]
        public async Task<IActionResult> GetPosts(int offset) => Ok(await Bl.GetPosts(offset).ConfigureAwait(false));

        [HttpGet, Route("get-categories")]
        public async Task<IActionResult> GetCategories(int offset) => Ok(await Bl.GetCategories(offset).ConfigureAwait(false));

        [HttpGet, Route("get-post-by-slug")]
        public async Task<IActionResult> GetPostBySlug(string slug) => Ok(await Bl.GetPostBySlug(slug).ConfigureAwait(false));

        [HttpGet, Route("get-category-by-slug")]
        public async Task<IActionResult> GetCategoryBySlug(string slug) => Ok(await Bl.GetCategoryBySlug(slug).ConfigureAwait(false));

        [HttpGet, Route("get-post-by-id")]
        public async Task<IActionResult> GetPostById(string id, bool richContent) => Ok(await Bl.GetPostById(id, richContent).ConfigureAwait(false));

        [HttpGet, Route("get-category-by-id")]
        public async Task<IActionResult> GetCategoryById(string id) => Ok(await Bl.GetCategoryById(id).ConfigureAwait(false));
    }
}
