using DTO.General.Base.Database;
using DTO.General.Image.Input;
using DTO.Hub.Application.Youtube.Input;
using System.Collections.Generic;

namespace DTO.Hub.Application.Youtube.Database
{
    public class YoutubePlaylist : BaseData
    {
        public YoutubePlaylist(string id, string name, string youtubeChannelId, string channelId, string allyId, ImageFormat img)
        {
            YoutubePlaylistName = name;
            YoutubePlaylistId = id;
            YoutubeChannelId = youtubeChannelId;
            ChannelId = channelId;
            AllyId = allyId;
            Image = img;
        }

        public YoutubePlaylist(HubYoutubePlaylistInput input, ImageFormat img)
        {
            if (input == null)
                return;

            YoutubePlaylistId = input.YoutubePlaylistId;
            YoutubePlaylistName = input.YoutubePlaylistName;
            YoutubeChannelId = input.YoutubeChannelId;
            ChannelId = input.ChannelId;
            AllyId = input.AllyId;
            Tags = input.Tags;
            Image = img;
            IsGlobal = input.IsGlobal;
        }

        public YoutubePlaylist(HubYoutubePlaylistInput input, ImageFormat img, string id)
        {
            if (input == null)
                return;

            Id = id;
            YoutubePlaylistId = input.YoutubePlaylistId;
            AllyId = input.AllyId;
            YoutubePlaylistName = input.YoutubePlaylistName;
            YoutubeChannelId = input.YoutubeChannelId;
            ChannelId = input.ChannelId;
            Tags = input.Tags;
            Image = img;
            IsGlobal = input.IsGlobal;
        }

        public string YoutubePlaylistName { get; set; }
        public string YoutubePlaylistId { get; set; }
        public string YoutubeChannelId { get; set; }
        public string ChannelId { get; set; }
        public string AllyId { get; set; }
        public ImageFormat Image { get; set; }
        public List<string> Tags { get; set; }
        public bool IsGlobal { get; set; }
    }
}
