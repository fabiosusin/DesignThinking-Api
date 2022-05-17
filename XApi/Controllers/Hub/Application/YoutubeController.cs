using Business.API.General.Youtube;
using Business.API.Hub.Application.Youtube;
using DAO.DBConnection;
using DTO.Hub.Application.Youtube.Input;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Hub.Application
{
    [ApiController]
    public class YoutubeController : ApplicationBaseController<YoutubeController>
    {
        private readonly BlHubYoutube BlHubYoutube;
        private readonly BlYoutubePlaylist BlYoutubePlaylist;
        private readonly BlYoutubeChannels BlYoutubeChannels;
        public YoutubeController(ILogger<YoutubeController> logger, XDataDatabaseSettings settings) : base(logger)
        {
            BlYoutubePlaylist = new(settings);
            BlHubYoutube = new(settings);
            BlYoutubeChannels = new(settings);
        }

        [HttpPost, Route("list")]
        public IActionResult ListPlaylists(HubYoutubeListInput input) => Ok(BlYoutubePlaylist.List(input));

        [HttpPost, Route("list-channels")]
        public IActionResult ListChannels(HubYoutubeListInput input) => Ok(BlYoutubeChannels.List(input));

        [HttpPost, Route("upsert-playlist")]
        public IActionResult RegisterPlaylist(HubYoutubePlaylistInput input) => Ok(BlHubYoutube.UpsertYoutubePlaylist(input));

        [HttpPost, Route("upsert-channel")]
        public IActionResult RegisterChannel(HubYoutubeChannelInput input) => Ok(BlHubYoutube.UpsertYoutubeChannel(input));

        [HttpPost, Route("upsert-ally-playlist")]
        public IActionResult UpsertAllyPlaylist(HubYoutubeAllyPlaylistInput input) => Ok(BlHubYoutube.UpsertAllyPlaylist(input));

        [HttpPost, Route("upsert-ally-channel")]
        public IActionResult UpsertAllyChannel(HubYoutubeAllyChannelInput input) => Ok(BlHubYoutube.UpsertAllyChannel(input));

        [HttpPost, Route("upsert-highlighted-ally-playlist/{id}")]
        public IActionResult UpsertAllyPlaylistHighlighted(string id) => Ok(BlHubYoutube.UpsertAllyPlaylistHighlighted(id));

        [HttpPost, Route("remove-playlist")]
        public IActionResult RemovePlaylist(string id, string allyId, string userId) => Ok(BlHubYoutube.RemovePlaylist(id, allyId, userId));

        [HttpPost, Route("remove-channel")]
        public IActionResult RemoveChannel(string id, string allyId, string userId) => Ok(BlHubYoutube.RemoveChannel(id, allyId, userId));
    }
}
