using Business.API.External.Youtube;
using DAO.DBConnection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.External.Wix
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Youtube")]
    [Route("v1/External/Youtube")]
    public class YoutubeController : BaseController<YoutubeController>
    {
        protected BlYoutube Bl;
        public YoutubeController(ILogger<YoutubeController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);

        [HttpPost, Route("register-playlists")]
        public IActionResult RegisterPlaylists(string allyId) => Ok(Bl.RegisterPlaylists(allyId));

        [HttpPost, Route("save-playlists")]
        public IActionResult SavePlaylists() => Ok(Bl.SavePlaylists());
    }
}
