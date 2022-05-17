using DAO.DBConnection;
using DAO.Hub.Application.Youtube;
using DAO.Integration.Youtube;
using DTO.General.Image.Input;
using DTO.Integration.Youtube.Output;
using Services.Integration.Youtube.Video;
using System;
using System.Linq;
using System.Threading.Tasks;
using Useful.Extensions;
using Utils.Extensions.Files.Images;

namespace Business.API.External.Youtube
{
    public class BlYoutube
    {
        private readonly YoutubeVideoService YoutubeVideoService;
        private readonly YoutubePlaylistDAO YoutubePlaylistDAO;
        private readonly YoutubeChannelsDAO YoutubeChannelsDAO;
        private readonly YoutubeCachedVideosDAO YoutubeCachedVideosDAO;
        public BlYoutube(XDataDatabaseSettings settings)
        {
            YoutubePlaylistDAO = new(settings);
            YoutubeVideoService = new(settings);
            YoutubeChannelsDAO = new(settings);
            YoutubeCachedVideosDAO = new(settings);
        }

        public async Task RegisterPlaylists(string allyId)
        {
            bool hasMore;
            var offset = 0;
            var pageToken = string.Empty;
            var channels = YoutubeChannelsDAO.List(new(new(allyId)));

            foreach (var channel in channels)
            {
                do
                {
                    var playlists = await GetPlaylists(pageToken, channel.YoutubeChannelId).ConfigureAwait(false);
                    if (playlists == null)
                        return;
                    pageToken = playlists.NextPageToken;
                    offset += 50;
                    hasMore = playlists.PageInfo.TotalResults > offset;

                    if (!(playlists?.Items?.Any() ?? false))
                        continue;

                    var existingsIds = YoutubePlaylistDAO.FindAllAllyPlaylistsIds(allyId);

                    foreach (var item in playlists.Items)
                    {
                        if (existingsIds.Contains(item.Id))
                            continue;

                        ImageFormat image = null;
                        try
                        {
                            image = ImageFactory.SaveListResolutions(await StringExtension.GetImageAsBase64Url(item.Snippet.Thumbnails.Maxres?.Url ?? item.Snippet.Thumbnails.High.Url).ConfigureAwait(false));
                        }
                        catch { }

                        _ = YoutubePlaylistDAO.Insert(new(item.Id, item.Snippet?.Title, channel.YoutubeChannelId, channel.Id, allyId, image));
                    }
                }
                while (hasMore);
            }
        }

        public async Task<YoutubePlaylistDataOutput> GetPlaylists(string pageToken, string channelId)
        {
            try
            {
                return await YoutubeVideoService.GetPlaylists(50, pageToken, channelId).ConfigureAwait(false);
            }
            catch { return null; }
        }

        public async Task<YoutubePlaylistItemDataOutput> GetVideos(string playlistId, string pageToken)
        {
            try
            {
                return await YoutubeVideoService.GetPlaylistVideosForService(playlistId, pageToken, 50).ConfigureAwait(false);
            }
            catch { return null; }
        }

        public async Task SavePlaylists()
        {
            bool hasMore;
            var playlists = YoutubePlaylistDAO.FindAll();

            foreach (var playlist in playlists)
            {
                var offset = 0;
                var pageToken = string.Empty;

                if (playlist.Tags?.Any() ?? false)
                    continue;

                do
                {
                    var videos = await GetVideos(playlist.YoutubePlaylistId, pageToken).ConfigureAwait(false);

                    if (videos == null)
                        return;

                    offset += 50;
                    hasMore = videos.PageInfo.TotalResults > offset;

                    if (!(videos?.Items?.Any() ?? false))
                        continue;

                    var existing = YoutubeCachedVideosDAO.FindPlaylistData(playlist.YoutubeChannelId, playlist.YoutubePlaylistId, videos.PrevPageToken, videos.NextPageToken);
                    if (existing != null)
                        YoutubeCachedVideosDAO.Update(new(existing.YoutubePlaylistId, existing.YoutubeChannelId, videos.NextPageToken, videos.PrevPageToken, pageToken, videos));
                    else
                        YoutubeCachedVideosDAO.Insert(new(playlist.YoutubePlaylistId, playlist.YoutubeChannelId, videos.NextPageToken, videos.PrevPageToken, pageToken, videos));
                    

                    pageToken = videos.NextPageToken;
                }
                while (hasMore);
            }
        }

    }
}
