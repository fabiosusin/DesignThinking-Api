using System;
using System.Collections.Generic;

namespace DTO.Integration.Youtube.Output
{
    public class YoutubePlaylistDataOutput
    {
        public string Kind { get; set; }
        public string Etag { get; set; }
        public string NextPageToken { get; set; }
        public YoutubePageInfoOutput PageInfo { get; set; }
        public List<YoutubePlaylistItem> Items { get; set; }
    }

    public class YoutubePlaylistItem
    {
        public string Kind { get; set; }
        public string Etag { get; set; }
        public string Id { get; set; }
        public YoutubePlaylistItemSnippet Snippet { get; set; }
    }

    public class YoutubePlaylistItemSnippet
    {
        public DateTime PublishedAt { get; set; }
        public string ChannelId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public YoutubePlaylistItemThumbnails Thumbnails { get; set; }
        public string ChannelTitle { get; set; }
        public YoutubePlaylistItemLocalized Localized { get; set; }
        public string DefaultLanguage { get; set; }
    }

    public class YoutubePlaylistItemLocalized
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class YoutubePlaylistItemThumbnails : YoutubeThumbnailsVideo
    {
        public YoutubeThumbnailData Default { get; set; }
    }
}
