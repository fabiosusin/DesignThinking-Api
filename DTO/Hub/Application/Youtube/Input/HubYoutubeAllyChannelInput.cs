namespace DTO.Hub.Application.Youtube.Input
{
    public class HubYoutubeAllyChannelInput
    {
        public HubYoutubeAllyChannelInput() { }
        public HubYoutubeAllyChannelInput(string allyId, string channelId)
        {
            AllyId = allyId;
            ChannelId = channelId;
        }

        public string AllyId { get; set; }
        public string ChannelId { get; set; }
        public bool Linked { get; set; }
    }
}
