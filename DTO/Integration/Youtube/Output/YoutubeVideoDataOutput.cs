using DTO.Hub.Application.Youtube.Database;
using System.Collections.Generic;

namespace DTO.Integration.Youtube.Output
{
    public class YoutubeVideoDataOutput
    {
        public YoutubeVideoDataOutput(YoutubeCachedVideos video)
        {
            Items = new();
            Items.Add(new(video.Data.Items[0]));
        }
        public string Kind { get; set; }
        public string Etag { get; set; }
        public string NextPageToken { get; set; }
        public string RegionCode { get; set; }
        public YoutubePageInfoOutput PageInfo { get; set; }
        public List<YoutubeVideoDataItem> Items { get; set; }
    }

    public class YoutubeVideoDataItem
    { 
        public YoutubeVideoDataItem(YoutubePlaylistItemData video)
        {
            Kind = video.Kind;
            Etag = video.Etag;
            Id = video.Id;
            Snippet = video.Snippet;
            Status = video.Status;
        }
        public string Kind { get; set; }
        public string Etag { get; set; }
        public string Id { get; set; }
        public YoutubeVideoSnippetOutput Snippet { get; set; }
        public YoutubeVideoStatusOutput Status { get; set; }
        public YoutubeVideoStatisticsOutput Statistics { get; set; }
    }

}
