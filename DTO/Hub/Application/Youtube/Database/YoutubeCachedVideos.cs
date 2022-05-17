using DTO.General.Base.Database;
using DTO.Integration.Youtube.Output;

namespace DTO.Hub.Application.Youtube.Database
{
    public class YoutubeCachedVideos : BaseData
    {
        public YoutubeCachedVideos() => Data = new();
        public YoutubeCachedVideos(string youtubePlaylistId, string youtubeChannelId, string nextPageToken, string prevPageToken, string pageToken, YoutubePlaylistItemDataOutput data)
        {
            YoutubePlaylistId = youtubePlaylistId;
            YoutubeChannelId = youtubeChannelId;
            NextPageToken = nextPageToken;
            PrevPageToken = prevPageToken;
            PageToken = pageToken;
            Data = data;
        }
        public string YoutubePlaylistId { get; set; }
        public string YoutubeChannelId { get; set; }
        public string NextPageToken { get; set; }
        public string PrevPageToken { get; set; }
        public string PageToken { get; set; }
        public YoutubePlaylistItemDataOutput Data { get; set; }
    }
}
