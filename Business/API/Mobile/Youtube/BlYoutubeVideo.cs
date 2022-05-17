using DAO.DBConnection;
using DTO.Integration.Youtube.Output;
using Services.Integration.Youtube.Video;
using System.Threading.Tasks;
using DAO.Hub.Application.Youtube;
using System.Linq;
using DTO.Mobile.Youtube.Output;
using System.Collections.Generic;
using DTO.Hub.Application.Youtube.Output;
using DTO.Hub.Application.Youtube.Input;
using DAO.Integration.Youtube;
using DTO.Hub.Application.Youtube.Database;

namespace Business.API.Mobile.Youtube
{
    public class BlYoutubeVideo
    {
        private readonly YoutubeVideoService YoutubeVideoService;
        private readonly YoutubePlaylistDAO YoutubePlaylistDAO;
        private readonly YoutubeAllyPlaylistDAO YoutubeAllyPlaylistDAO;
        private readonly YoutubeAllyChannelDAO YoutubeAllyChannelDAO;
        private readonly YoutubeChannelsDAO YoutubeChannelsDAO;

        public BlYoutubeVideo(XDataDatabaseSettings settings)
        {
            YoutubePlaylistDAO = new(settings);
            YoutubeVideoService = new(settings);
            YoutubeAllyPlaylistDAO = new(settings);
            YoutubeAllyChannelDAO = new(settings);
            YoutubeChannelsDAO = new(settings);
        }

        public YoutubeListPlaylistsOutput List(HubYoutubeListInput input)
        {
            if (input == null)
                return new("Input não informado!");

            if (input.Filters == null)
                return new("Nenhum filtro informado!");

            return input.Filters.ListType switch
            {
                HubYoutubeListType.Old => GetOldList(input),
                HubYoutubeListType.Channels => GetChannels(input),
                HubYoutubeListType.Playlist => GetPlaylists(input),
                _ => null
            };
        }

        public async Task<YoutubePlaylistDataOutput> GetPlaylists(int max, string pageToken, string channelId)
        {
            try
            {
                return await YoutubeVideoService.GetPlaylists(max, pageToken, channelId).ConfigureAwait(false);
            }
            catch { return null; }
        }

        public AppYoutubeVideosDataOutput GetPlaylistVideos(string playlistId, string pageToken, int max)
        {
            try
            {
                var playlist = YoutubePlaylistDAO.FindById(playlistId);
                return !(playlist?.Tags?.Any() ?? false) ? new(YoutubeVideoService.GetPlaylistVideos(playlist?.YoutubePlaylistId, pageToken, max)) :
                        new(GetVideosByTags(playlist, max));
            }
            catch { return null; }
        }

        public YoutubeVideoDataOutput GetVideo(string videoId)
        {
            try
            {
                return YoutubeVideoService.GetVideo(videoId);
            }
            catch { return null; }
        }

        public YoutubeDataOutput GetVideos(string search, int max, string allyId)
        {
            try
            {
                var allyChannels = YoutubeAllyChannelDAO.Find(x => x.AllyId == allyId).Select(x => x.ChannelId).ToList();
                var allyYoutubeChannelsIds = allyChannels.Select(y => YoutubeChannelsDAO.FindOne(x => x.Id == y).YoutubeChannelId).ToList();
                YoutubeDataOutput result = null;

                foreach (var channel in allyYoutubeChannelsIds)
                {
                    try
                    {
                        var videos = YoutubeVideoService.GetVideos(search, max, channel);
                        if (result == null)
                            result = videos;
                        else
                        {
                            var diff = max - (result.Items.Count + videos.Items.Count);
                            if (diff < 0)
                            {
                                var items = videos.Items.Take(max - result.Items.Count);
                                result.Items.AddRange(items);
                                break;
                            }
                            else
                                result.Items.AddRange(videos.Items);
                        }
                    }
                    catch { }
                }

                return result;
            }
            catch { return null; }
        }

        public YoutubePlaylistItemDataOutput GetVideosByTags(YoutubePlaylist playlist, int max) => YoutubeVideoService.GetVideosByTags(playlist?.Tags, max, playlist?.YoutubeChannelId, playlist?.YoutubePlaylistId);

        public YoutubeDataOutput GetLastVideos(int max, string allyId)
        {
            try
            {
                var channelId = YoutubeAllyChannelDAO.FindOne(x => x.AllyId == allyId)?.ChannelId;
                var youtubeChannelId = YoutubeChannelsDAO.FindOne(x => x.Id == channelId)?.YoutubeChannelId;

                if (youtubeChannelId == null)
                    return null;

                return YoutubeVideoService.GetLastVideos(max, youtubeChannelId);
            }
            catch { return null; }
        }

        public async Task<YoutubeDataOutput> GetLives(string allyId)
        {
            try
            {
                var result = new YoutubeDataOutput();
                var allyChannels = YoutubeAllyChannelDAO.Find(x => x.AllyId == allyId).Select(x => x.ChannelId).ToList();
                var allyYoutubeChannelsIds = allyChannels.Select(y => YoutubeChannelsDAO.FindOne(x => x.Id == y).YoutubeChannelId).ToList();

                foreach (var allyChannel in allyYoutubeChannelsIds)
                {
                    var liveStream = await YoutubeVideoService.GetLives(allyChannel).ConfigureAwait(false);
                    if (liveStream?.Items?.Any() ?? false)
                        return liveStream;
                }

                return result;
            }
            catch { return null; }
        }

        public HubYoutubeHighlightedListOutput GetHighlightedList(string allyId)
        {
            if (string.IsNullOrEmpty(allyId))
                return new("Nenhum aliado informado!");

            var allyPlaylist = YoutubeAllyPlaylistDAO.FindOne(x => x.AllyId == allyId && x.Highlighted == true);

            return new(allyPlaylist == null ?
                new YoutubeHighlightedListData((GetLastVideos(5, allyId))?.Items) :
                new YoutubeHighlightedListData(allyPlaylist.PlaylistId, (GetPlaylistVideos(allyPlaylist.PlaylistId, null, 5))?.Items));
        }

        private YoutubeListPlaylistsOutput GetOldList(HubYoutubeListInput input)
        {
            if (string.IsNullOrEmpty(input.Filters.AllyId))
                return new("AllyId não informado!");

            var allyChannels = YoutubeAllyChannelDAO.Find(x => x.AllyId == input.Filters.AllyId).Select(x => x.ChannelId);
            input.Filters.ChannelsIds = allyChannels?.Select(y => YoutubeChannelsDAO.FindOne(x => x.Id == y).YoutubeChannelId)?.ToList();

            var result = YoutubePlaylistDAO.List(input);
            if (!(result?.Any() ?? false))
                return new("Nenhuma Playlist encontrada!");

            var playlists = new List<YoutubeListPlaylistsSubChannelOutput>();

            var mainChannels = result.Where(x => !(x.Tags?.Any() ?? false));
            foreach (var item in mainChannels)
            {
                playlists.Add(new YoutubeListPlaylistsSubChannelOutput
                {
                    YoutubePlaylistName = item.YoutubePlaylistName,
                    YoutubePlaylistId = item.YoutubePlaylistId,
                    AllyId = item.AllyId,
                    Highlighted = item.Highlighted,
                    Id = item.Id,
                    ImageUrl = item.ImageUrl,
                    Linked = item.Linked,
                    Tags = item.Tags
                });
            };

            var subChannels = result.Where(x => x.Tags?.Any() ?? false);
            foreach (var item in subChannels)
            {
                var existing = playlists.FirstOrDefault(x => x.YoutubePlaylistId == item.YoutubePlaylistId && !(x.Tags?.Any() ?? false));
                if (existing == null)
                    continue;

                existing.SubChannels.Add(item);
            }

            return new(playlists);
        }

        private YoutubeListPlaylistsOutput GetChannels(HubYoutubeListInput input)
        {
            if (string.IsNullOrEmpty(input.Filters.AllyId))
                return new("AllyId não informado!");

            var allyChannels = YoutubeAllyChannelDAO.Find(x => x.AllyId == input.Filters.AllyId).Select(x => x.ChannelId);
            input.Filters.ChannelsIds = allyChannels?.Select(y => YoutubeChannelsDAO.FindOne(x => x.Id == y).YoutubeChannelId)?.ToList();

            var result = YoutubePlaylistDAO.List(input);
            return !(result?.Any() ?? false) ?
                new("Nenhuma Playlist encontrada!") :
                new(result.Where(x => !(x.Tags?.Any() ?? false)));
        }

        private YoutubeListPlaylistsOutput GetPlaylists(HubYoutubeListInput input)
        {
            if (string.IsNullOrEmpty(input.Filters.PlaylistId))
                return new("PlaylistId não informado!");

            input.Filters.PlaylistId = YoutubePlaylistDAO.FindById(input.Filters.PlaylistId)?.YoutubePlaylistId;

            var result = YoutubePlaylistDAO.List(input);
            if (!(result?.Any() ?? false))
                return new("Nenhuma Playlist encontrada!");

            return result.Count == 1 && !(result.FirstOrDefault()?.Tags?.Any() ?? false) ?
                new(result) :
                new(result.Where(x => x.Tags?.Any() ?? false));
        }
    }
}
