using System.Collections.Generic;

namespace DTO.Hub.Application.Youtube.Input
{
    public class HubYoutubePlaylistInput
    {
        public string AllyId { get; set; }
        public string PlaylistId { get; set; }
        public string YoutubePlaylistName { get; set; }
        public string YoutubePlaylistId { get; set; }
        public string YoutubeChannelId { get; set; }
        public string ChannelId { get; set; }
        public string ImgBase64 { get; set; }
        public string ImgUrl { get; set; }
        public List<string> Tags { get; set; }
        public bool IsGlobal { get; set; }
    }
}
