using DTO.General.Base.Api.Output;
using DTO.Hub.Application.Youtube.Database;
using System.Collections.Generic;

namespace DTO.Hub.Application.Youtube.Output
{
    public class HubYoutubePlaylistListOutput : BaseApiOutput
    {
        public HubYoutubePlaylistListOutput(string msg) : base(msg) { }
        public HubYoutubePlaylistListOutput(IEnumerable<YoutubePlaylistListData> playlists) : base(true) => Playlists = playlists;
        public IEnumerable<YoutubePlaylistListData> Playlists { get; set; }
    }

    public class YoutubePlaylistListData
    {
        public YoutubePlaylistListData() { }

        public YoutubePlaylistListData(YoutubePlaylist input, string img, bool linked, bool? highlighted)
        {
            if (input == null)
                return;

            Id = input.Id;
            AllyId = input.AllyId;
            YoutubePlaylistName = input.YoutubePlaylistName;
            YoutubePlaylistId = input.YoutubePlaylistId;
            YoutubeChannelId = input.YoutubeChannelId;
            ImageUrl = img;
            Linked = linked;
            Highlighted = highlighted ?? false;
            Tags = input.Tags;
            IsGlobal = input.IsGlobal;
        }

        public string Id { get; set; }
        public string AllyId { get; set; }
        public string YoutubePlaylistName { get; set; }
        public string YoutubePlaylistId { get; set; }
        public string YoutubeChannelId { get; set; }
        public string ImageUrl { get; set; }
        public bool Linked { get; set; }
        public bool Highlighted { get; set; }
        public List<string> Tags { get; set; }
        public bool IsGlobal { get; set; }
    }
}
