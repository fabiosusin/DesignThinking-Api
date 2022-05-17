using DAO.DBConnection;
using DAO.General.Log;
using DAO.Integration.Youtube;
using DTO.General.Api.Enum;
using DTO.General.Log.Database;
using DTO.General.Log.Enum;
using DTO.Integration.Youtube.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Integration.Youtube
{
    internal class YoutubeApiService
    {
        private readonly LogHistoryDAO LogHistoryDAO;
        private readonly YoutubeApiKeyDAO YoutubeApiKeyDAO;
        private readonly ApiDispatcher _apiDispatcher;
        private static readonly string BaseUrl = "https://www.googleapis.com/youtube/v3";

        public YoutubeApiService(XDataDatabaseSettings settings)
        {
            LogHistoryDAO = new(settings);
            YoutubeApiKeyDAO = new(settings);
            _apiDispatcher = new ApiDispatcher();
        }

        public async Task<YoutubePlaylistDataOutput> GetPlaylists(int max, string pageToken, string channelId) =>
            await SendRequest<YoutubePlaylistDataOutput>(RequestMethodEnum.GET, $"{ BaseUrl }/playlists?part=snippet&maxResults={max}{(string.IsNullOrEmpty(pageToken) ? "" : $"&pageToken={pageToken}")}&channelId={channelId}");

        public async Task<YoutubePlaylistItemDataOutput> GetPlaylistVideos(string playlistId, string pageToken, int max) =>
            await SendRequest<YoutubePlaylistItemDataOutput>(RequestMethodEnum.GET, $"{ BaseUrl }/playlistItems?part=snippet&maxResults={max}&order=date&playlistId={playlistId}{(string.IsNullOrEmpty(pageToken) ? "" : $"&pageToken={pageToken}")}");

        public async Task<YoutubeVideoDataOutput> GetVideo(string videoId) =>
            await SendRequest<YoutubeVideoDataOutput>(RequestMethodEnum.GET, $"{ BaseUrl }/videos?part=snippet,statistics&id={videoId}&order=date");

        public async Task<YoutubeDataOutput> GetVideos(string search, int results, string channelId) =>
            await SendRequest<YoutubeDataOutput>(RequestMethodEnum.GET, $"{ BaseUrl }/search?maxResults={results}&part=snippet&channelId={channelId}&order=date&q={Uri.EscapeDataString(search)}");

        public async Task<YoutubeDataOutput> GetVideosByTags(List<string> tags, int results, string channelId)
        {
            var result = await SendRequest<YoutubeDataOutput>(RequestMethodEnum.GET, $"{ BaseUrl }/search?maxResults={results}&part=snippet&channelId={channelId}&q={Uri.EscapeDataString($"##[\"{string.Join(',', tags?.Select(x => x)) }\"]")}");

            if (result?.Items?.Any() ?? false)
                result.Items = result.Items.OrderByDescending(x => x.Snippet.PublishedAt).ToList();

            return result;
        }

        public async Task<YoutubeDataOutput> GetLastVideos(int results, string channelId) =>
            await SendRequest<YoutubeDataOutput>(RequestMethodEnum.GET, $"{ BaseUrl }/search?maxResults={results}&part=snippet&channelId={channelId}&order=date");

        public async Task<YoutubeDataOutput> GetLives(string channelId) =>
            await SendRequest<YoutubeDataOutput>(RequestMethodEnum.GET, $"{ BaseUrl }/search?part=snippet&channelId={channelId}&eventType=live&type=video&order=date");

        private async Task<T> SendRequest<T>(RequestMethodEnum method, string url, object body = null)
        {
        Retry:
            var youtubeKeyDto = YoutubeApiKeyDAO.GetValidKey();
            if (youtubeKeyDto == null)
                throw new Exception("Nenhuma Chave Válida encontrada");

            try
            {
                var key = $"&key={youtubeKeyDto.Key}";

                return await _apiDispatcher.DispatchWithResponseAsync<T>(url + key, method, body);
            }
            catch (Exception e)
            {
                if (e.Message.ToLower().Contains("quota"))
                {
                    YoutubeApiKeyDAO.MarkAsExpired(youtubeKeyDto);
                    goto Retry;
                }
                
                LogHistoryDAO.Insert(new AppLogHistory
                {
                    Message = "Erro ao buscar Playlists no Youtube!",
                    ExceptionMessage = e.Message,
                    Type = AppLogTypeEnum.XApiYoutubeRequestError,
                    Method = "GetPlaylists",
                    Date = DateTime.Now
                });
                throw;
            }
        }
    }
}
