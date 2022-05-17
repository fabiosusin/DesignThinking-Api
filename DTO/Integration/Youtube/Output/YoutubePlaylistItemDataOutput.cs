using DTO.Hub.Application.Youtube.Database;
using System;
using System.Collections.Generic;

namespace DTO.Integration.Youtube.Output
{
    public class YoutubePlaylistItemDataOutput
    {
        public YoutubePlaylistItemDataOutput() => Items = new List<YoutubePlaylistItemData>();
        public YoutubePlaylistItemDataOutput(YoutubeCachedVideos input)
        {
            if (input == null)
                return;

            Kind = input.Data.Kind;
            Etag = input.Data.Etag;
            NextPageToken = input.Data.NextPageToken;
            PrevPageToken =  input.Data.PrevPageToken;
            PageInfo = input.Data.PageInfo;
            Items = input.Data.Items;
        }
        public string Kind { get; set; }
        public string Etag { get; set; }
        public string NextPageToken { get; set; }
        public string PrevPageToken { get; set; }
        public YoutubePageInfoOutput PageInfo { get; set; }
        public List<YoutubePlaylistItemData> Items { get; set; }
    }

    public class YoutubePlaylistItemData
    {
        public string Id { get; set; }
        public string Kind { get; set; }
        public string Etag { get; set; }
        public YoutubePlaylistItemDataSnippet Snippet { get; set; }
        public YoutubeVideoStatusOutput Status { get; set; }
        public YoutubeItemContentDetails ContentDetails { get; set; }
    }

    public class YoutubePlaylistItemDataSnippet : YoutubeVideoSnippetOutput
    {
        public YoutubePlaylistItemDataSnippet(YoutubeVideoSnippetOutput input)
        {
            if (input == null)
                return;

            PublishedAt = input.PublishedAt;
            ChannelId = input.ChannelId;
            Title = input.Title;
            Description = input.Description;
            Thumbnails = input.Thumbnails;
            ResourceId = input.ResourceId;

        }

        public string ChannelTitle { get; set; }
        public string PlaylistId { get; set; }
        public int Position { get; set; }
    }

    public class YoutubeItemContentDetails
    {
        public string StartAt { get; set; }
        public string EndAt { get; set; }
        public string VideoId { get; set; }
        public string Note { get; set; }
    }
}
