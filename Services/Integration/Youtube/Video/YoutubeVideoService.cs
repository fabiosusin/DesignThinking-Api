using DAO.DBConnection;
using DTO.Integration.Youtube.Output;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Integration.Youtube.Video
{
    public class YoutubeVideoService
    {
        internal YoutubeApiService YoutubeApiService;
        private readonly YoutubeVideoCacheService YoutubeVideoCacheService;
        public YoutubeVideoService(XDataDatabaseSettings settings) 
        {
            YoutubeApiService = new(settings);
            YoutubeVideoCacheService = new(settings);
        } 

        public async Task<YoutubePlaylistDataOutput> GetPlaylists(int max, string pageToken, string channelId ) => await YoutubeApiService.GetPlaylists(max, pageToken, channelId).ConfigureAwait(false);

        public YoutubePlaylistItemDataOutput GetPlaylistVideos(string input, string pageToken, int max) => YoutubeVideoCacheService.GetPlaylistVideos(input, pageToken, max);

        public async Task<YoutubePlaylistItemDataOutput> GetPlaylistVideosForService(string input, string pageToken, int max) => await YoutubeApiService.GetPlaylistVideos(input, pageToken, max).ConfigureAwait(false);

        public YoutubeVideoDataOutput GetVideo(string input) => YoutubeVideoCacheService.GetVideo(input);

        public YoutubeDataOutput GetVideos(string search, int max, string channelId) => YoutubeVideoCacheService.GetVideos(search, max, channelId);

        public YoutubePlaylistItemDataOutput GetVideosByTags(List<string> tags, int max, string channelId, string playlistId) => YoutubeVideoCacheService.GetVideosByTags(tags, max, channelId, playlistId);

        public YoutubeDataOutput GetLastVideos(int max, string channelId) => YoutubeVideoCacheService.GetLastVideos(max, channelId);

        public async Task<YoutubeDataOutput> GetLives(string channelId) => await YoutubeApiService.GetLives(channelId).ConfigureAwait(false);
    }
}
