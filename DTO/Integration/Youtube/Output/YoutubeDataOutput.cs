using DTO.Hub.Application.Youtube.Database;
using System.Collections.Generic;
using System.Linq;

namespace DTO.Integration.Youtube.Output
{
    public class YoutubeDataOutput
    {
        public YoutubeDataOutput() { }
        public YoutubeDataOutput(YoutubeCachedVideos input)
        {
            if (input == null)
                return;

            Kind = input.Data.Kind;
            Etag = input.Data.Etag;
            NextPageToken = input.Data.NextPageToken;
            PageInfo = input.Data.PageInfo;
            Items = input.Data.Items.Select(x => new YoutubeDataItem(x)).ToList();
        }
        public string Kind { get; set; }
        public string Etag { get; set; }
        public string NextPageToken { get; set; }
        public YoutubePageInfoOutput PageInfo { get; set; }
        public List<YoutubeDataItem> Items { get; set; }
    }

    public class YoutubeDataItem
    {
        public YoutubeDataItem(YoutubePlaylistItemData input)
        {
            if (input == null)
                return;

            Kind = input.Kind;
            Etag = input.Etag;
            Id = new(input.Kind, input.Id);
            Snippet = input.Snippet;
            Status = input.Status;
        }
        public string Kind { get; set; }
        public string Etag { get; set; }
        public YoutubeDataVideoId Id { get; set; }
        public YoutubeVideoSnippetOutput Snippet { get; set; }
        public YoutubeVideoStatusOutput Status { get; set; }
        public YoutubeVideoStatisticsOutput Statistics { get; set; }
    }

    public class YoutubeDataVideoId
    {
        public YoutubeDataVideoId(string kind, string videoId)
        {
            Kind = kind;
            VideoId = videoId;
        }
        public string Kind { get; set; }
        public string VideoId { get; set; }
    }
}
