using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Hub.Application.Youtube.Input
{
    public class HubYoutubeChannelInput
    {
        public string AllyId { get; set; }
        public string ChannelId { get; set; }
        public string ChannelName { get; set; }
        public string YoutubeChannelId { get; set; }
        public bool IsGlobal { get; set; }
    }
}
