using DTO.General.Image.Input;
using System.Collections.Generic;

namespace DTO.Hub.Application.Youtube.Input
{
    public class HubYoutubeFiltersInput
    {
        public HubYoutubeFiltersInput() { }
        public HubYoutubeFiltersInput(string allyId) => AllyId = allyId;
        public string PlaylistId { get; set; }
        public bool OnlyLinked { get; set; }
        public string AllyId { get; set; }
        public string Name { get; set; }
        public List<string> Tags { get; set; }
        public ListResolutionsSize ImageSize { get; set; }
        public List<string> ChannelsIds { get; set; }
        public HubYoutubeListType ListType { get; set; }
    }

    public enum HubYoutubeListType
    {
        Old,
        Channels,
        Playlist
    }
}
