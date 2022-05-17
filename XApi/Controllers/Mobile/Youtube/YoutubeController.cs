using Business.API.General.Youtube;
using Business.API.Mobile.Youtube;
using DAO.DBConnection;
using DTO.API.Auth;
using DTO.Hub.Application.Youtube.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace XApi.Controllers.Mobile.Youtube
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Youtube"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Mobile/Youtube")]
    public class YoutubeController : BaseController<YoutubeController>
    {
        protected BlYoutubeVideo Bl;
        protected BlYoutubePlaylist BlYoutubePlaylist;
        public YoutubeController(ILogger<YoutubeController> logger, XDataDatabaseSettings settings) : base(logger)
        {
            Bl = new(settings);
            BlYoutubePlaylist = new(settings);
        }

        [HttpPost, Route("list")]
        public IActionResult List(HubYoutubeListInput input) => Ok(Bl.List(input));

        [HttpGet, Route("get-highlighted-playlist/{id}")]
        public IActionResult GetHighlighted(string id) => Ok(Bl.GetHighlightedList(id));

        [HttpGet, Route("get-playlists")]
        public async Task<IActionResult> GetPlaylists(int max, string pageToken, string channelId) => Ok(await Bl.GetPlaylists(max, pageToken, channelId).ConfigureAwait(false));

        [HttpGet, Route("get-playlist-videos")]
        public IActionResult GetPlaylistVideos(string playlistId, string pageToken, int max) => Ok(Bl.GetPlaylistVideos(playlistId, pageToken, max));

        [HttpGet, Route("get-video")]
        public IActionResult GetVideo(string videoId) => Ok(Bl.GetVideo(videoId));

        [HttpGet, Route("get-videos")]
        public IActionResult GetVideos(string search, int max, string allyId) => Ok(Bl.GetVideos(search, max, allyId));

        [HttpGet, Route("get-last-videos")]
        public IActionResult GetLastVideos(int max, string allyId) => Ok(Bl.GetLastVideos(max, allyId));

        [HttpGet, Route("get-lives")]
        public async Task<IActionResult> GetLives(string allyId) => Ok(await Bl.GetLives(allyId).ConfigureAwait(false));
    }
}
