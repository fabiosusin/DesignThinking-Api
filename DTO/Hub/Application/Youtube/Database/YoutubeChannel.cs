using DTO.General.Base.Database;
using DTO.Hub.Application.Youtube.Input;

namespace DTO.Integration.Youtube.Database
{
    public class YoutubeChannel : BaseData
    {
        public YoutubeChannel(HubYoutubeChannelInput input)
        {
            if (input == null)
                return;

            YoutubeChannelId = input.YoutubeChannelId;
            AllyId = input.AllyId;
            YoutubeChannelName = input.ChannelName;
            IsGlobal = input.IsGlobal;
        }
        public YoutubeChannel(HubYoutubeChannelInput input, string id)
        {
            if (input == null)
                return;

            Id = id;
            YoutubeChannelId = input.YoutubeChannelId;
            AllyId = input.AllyId;
            YoutubeChannelName = input.ChannelName;
            IsGlobal = input.IsGlobal;
        }

        public string YoutubeChannelId { get; set; }
        public string AllyId { get; set; }
        public string YoutubeChannelName { get; set; }
        public bool IsGlobal { get; set; }
    }
}
