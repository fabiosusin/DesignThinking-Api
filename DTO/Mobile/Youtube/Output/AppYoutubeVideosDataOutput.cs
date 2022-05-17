using DTO.Integration.Youtube.Output;
using System.Collections.Generic;
using System.Linq;

namespace DTO.Mobile.Youtube.Output
{
    public class AppYoutubeVideosDataOutput
    {
        public AppYoutubeVideosDataOutput(YoutubeDataOutput input)
        {
            if (input == null)
                return;

            Kind = input.Kind;
            Etag = input.Etag;
            NextPageToken = input.NextPageToken;
            PageInfo = input.PageInfo;
            Items = input.Items?.Select(x => new MobileYoutubeVideo(x));
        }
        public AppYoutubeVideosDataOutput(YoutubePlaylistItemDataOutput input)
        {
            if (input == null)
                return;

            Kind = input.Kind;
            Etag = input.Etag;
            NextPageToken = input.NextPageToken;
            PrevPageToken = input.PrevPageToken;
            PageInfo = input.PageInfo;
            Items = input.Items?.Select(x => new MobileYoutubeVideo(x));
        }

        public string Kind { get; set; }
        public string Etag { get; set; }
        public string NextPageToken { get; set; }
        public string PrevPageToken { get; set; }
        public YoutubePageInfoOutput PageInfo { get; set; }
        public IEnumerable<MobileYoutubeVideo> Items { get; set; }
    }

    public class MobileYoutubeVideo
    {
        public MobileYoutubeVideo(YoutubeDataItem input)
        {
            if (input == null)
                return;

            Id = input.Id?.VideoId;
            Snippet = new(input.Snippet);
        }

        public MobileYoutubeVideo(YoutubePlaylistItemData input)
        {
            if (input == null)
                return;

            Id = input.Id;
            Snippet = input.Snippet;
        }

        public string Id { get; set; }
        public YoutubePlaylistItemDataSnippet Snippet { get; set; }
    }
}
