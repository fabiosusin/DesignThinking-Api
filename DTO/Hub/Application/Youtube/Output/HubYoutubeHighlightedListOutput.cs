using DTO.General.Base.Api.Output;
using DTO.Integration.Youtube.Output;
using DTO.Mobile.Youtube.Output;
using System.Collections.Generic;
using System.Linq;

namespace DTO.Hub.Application.Youtube.Output
{
    public class HubYoutubeHighlightedListOutput : BaseApiOutput
    {
        public HubYoutubeHighlightedListOutput(string msg) : base(msg) { }
        public HubYoutubeHighlightedListOutput(YoutubeHighlightedListData playlist) : base(true) => Playlist = playlist;
        public YoutubeHighlightedListData Playlist { get; set; }
    }

    public class YoutubeHighlightedListData
    {
        public YoutubeHighlightedListData(string playlistId, IEnumerable<MobileYoutubeVideo> videos)
        {
            PlaylistId = playlistId;
            if (!(videos?.Any() ?? false))
                return;

            Items = videos.Select(x => new YoutubeHighlightedItemData(x));
        }

        public YoutubeHighlightedListData(IEnumerable<YoutubeDataItem> videos)
        {
            if (!(videos?.Any() ?? false))
                return;

            Items = videos.Select(x => new YoutubeHighlightedItemData(x));
        }

        public string PlaylistId { get; set; }
        public IEnumerable<YoutubeHighlightedItemData> Items { get; set; }
    }

    public class YoutubeHighlightedItemData
    {
        public YoutubeHighlightedItemData(MobileYoutubeVideo input)
        {
            if (input?.Snippet == null)
                return;

            if (input.Snippet.Thumbnails != null)
                ImageUrl = input.Snippet.Thumbnails.Medium?.Url ?? input.Snippet.Thumbnails.High?.Url ?? input.Snippet.Thumbnails.Standard?.Url ?? input.Snippet.Thumbnails.Medium?.Url;

            VideoId = input.Snippet.ResourceId?.VideoId;
            Title = input.Snippet.Title;
            Description = input.Snippet.Description;
        }

        public YoutubeHighlightedItemData(YoutubeDataItem input)
        {
            if (input?.Snippet == null)
                return;

            if (input.Snippet.Thumbnails != null)
                ImageUrl = input.Snippet.Thumbnails.Medium?.Url ?? input.Snippet.Thumbnails.High?.Url ?? input.Snippet.Thumbnails.Standard?.Url ?? input.Snippet.Thumbnails.Medium?.Url;

            VideoId = input.Id?.VideoId;
            Title = input.Snippet.Title;
            Description = input.Snippet.Description;
        }

        public string VideoId { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

    }
}
