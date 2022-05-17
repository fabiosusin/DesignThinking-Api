using DTO.General.Base.Api.Output;
using DTO.Hub.Application.Youtube.Database;
using DTO.Integration.Youtube.Database;
using System.Collections.Generic;

namespace DTO.Hub.Application.Youtube.Output
{
    public class HubYoutubeChannelsListOutput : BaseApiOutput
    {
        public HubYoutubeChannelsListOutput(string msg) : base(msg) { }
        public HubYoutubeChannelsListOutput(IEnumerable<YoutubeChannelsListData> channels) : base(true) => Channels = channels;
        public IEnumerable<YoutubeChannelsListData> Channels { get; set; }
    }

    public class YoutubeChannelsListData
    {
        public YoutubeChannelsListData(YoutubeChannel input, bool linked)
        {
            Linked = linked;

            if (input == null)
                return;

            Id = input.Id;
            YoutubeChannelId = input.YoutubeChannelId;
            AllyId = input.AllyId;
            YoutubeChannelName = input.YoutubeChannelName;
            IsGlobal = input.IsGlobal;
        }

        public string Id { get; set; }
        public string YoutubeChannelId { get; set; }
        public string AllyId { get; set; }
        public string YoutubeChannelName { get; set; }
        public bool Linked { get; set; }
        public bool IsGlobal { get; set; }
    }
}

