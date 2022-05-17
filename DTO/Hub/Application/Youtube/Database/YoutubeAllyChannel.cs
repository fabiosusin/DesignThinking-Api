using DTO.General.Base.Database;

namespace DTO.Hub.Application.Youtube.Database
{
    public class YoutubeAllyChannel : BaseData
    {
        public YoutubeAllyChannel() { }
        public YoutubeAllyChannel(string allyId, string channelId)
        {
            AllyId = allyId;
            ChannelId = channelId;
        }

        public string AllyId { get; set; }
        public string ChannelId { get; set; }
    }
}
