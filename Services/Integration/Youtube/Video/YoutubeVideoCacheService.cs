using DAO.DBConnection;
using DAO.Hub.Application.Youtube;
using DTO.Hub.Application.Youtube.Database;
using DTO.Integration.Youtube.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Services.Integration.Youtube.Video
{
    public class YoutubeVideoCacheService
    {
        private readonly YoutubeCachedVideosDAO YoutubeSavedVideosDAO;
        public YoutubeVideoCacheService(XDataDatabaseSettings settings)
        {
            YoutubeSavedVideosDAO = new(settings);
        }

        public YoutubePlaylistItemDataOutput GetPlaylistVideos(string playlistId, string pageToken, int max)
        {
            var result = pageToken == null ? YoutubeSavedVideosDAO.FindOne(x => x.YoutubePlaylistId == playlistId && x.PrevPageToken == pageToken) : YoutubeSavedVideosDAO.FindOne(x => x.YoutubePlaylistId == playlistId && x.PageToken == pageToken);

            if (result == null)
                return null;

            return new(result);
        }

        public YoutubeDataOutput GetLastVideos(int results, string channelId)
        {
            var result = YoutubeSavedVideosDAO.FindOne(x => x.YoutubeChannelId == channelId);
            if (result?.Data?.Items?.Any() ?? false)
            {
                result.Data.Items = result?.Data.Items.Take(results).ToList();

                // TODO: Remover após a versão 1.8.0 do app estiver no ar
                // Feito isso apenas para retrocompatibilidade com as versões anteriores do App
                foreach (var item in result?.Data.Items)
                    item.Id = item.Snippet?.ResourceId?.VideoId;
            }

            return new(result);
        }

        public YoutubePlaylistItemDataOutput GetVideosByTags(List<string> tags, int results, string channelId, string playlistId)
        {
            var result = new YoutubeCachedVideos();
            var playlists = YoutubeSavedVideosDAO.Find(x => x.YoutubeChannelId == channelId && x.YoutubePlaylistId == playlistId);
            if (!(playlists?.Any() ?? false))
                return new(result);

            foreach (var playlist in playlists)
            {
                foreach (var video in playlist?.Data?.Items?.Where(x => !string.IsNullOrEmpty(x.Snippet?.Description)))
                {
                    if (tags.FirstOrDefault(x => video.Snippet.Description.ToLower().Contains(x.ToLower())) != null)
                        result.Data.Items.Add(video);

                    if (result.Data.Items.Count == results)
                        return new(result);
                }
            }

            return new(result);
        }

        public YoutubeVideoDataOutput GetVideo(string videoId)
        {
            var playlists = YoutubeSavedVideosDAO.FindAll();

            foreach (var playlist in playlists)
            {
                var video = playlist.Data.Items.FirstOrDefault(x => x.Id == videoId);
                if (video == null)
                    continue;

                // TODO: Remover após a versão 1.8.0 do app estiver no ar
                // Feito isso apenas para retrocompatibilidade com as versões anteriores do App
                video.Id = video.Snippet?.ResourceId?.VideoId;

                playlist.Data.Items.Clear();
                playlist.Data.Items.Add(video);
                return new(playlist);
            }

            return null;
        }

        public YoutubeDataOutput GetVideos(string search, int results, string channelId)
        {
            var videos = YoutubeSavedVideosDAO.Find(x => x.YoutubeChannelId == channelId);

            var result = videos.FirstOrDefault();
            result.Data.Items.Clear();

            foreach (var block in videos)
            {
                foreach (var video in block.Data.Items.Where(x => x.Snippet.Description.Contains(search) || x.Snippet.Title.Contains(search)))
                {
                    result.Data.Items.Add(video);

                    if (result.Data.Items.Count == results)
                        return new(result);
                }
            }

            return new(result);
        }
    }
}
