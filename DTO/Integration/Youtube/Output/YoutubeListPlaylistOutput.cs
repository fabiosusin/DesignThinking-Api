using DTO.General.Base.Api.Output;
using DTO.Hub.Application.Youtube.Output;
using System.Collections.Generic;
using System.Linq;

namespace DTO.Integration.Youtube.Output
{
    public class YoutubeListPlaylistsOutput : BaseApiOutput
    {
        public YoutubeListPlaylistsOutput(string msg) : base(msg) { }
        public YoutubeListPlaylistsOutput(IEnumerable<YoutubeListPlaylistsSubChannelOutput> playlists) : base(true) => Playlists = playlists;
        public YoutubeListPlaylistsOutput(IEnumerable<YoutubePlaylistListData> input) : base(true)
        {
            if (!(input?.Any() ?? false))
                return;

            var playlists = new List<YoutubeListPlaylistsSubChannelOutput>();
            foreach (var item in input)
            {
                playlists.Add(new YoutubeListPlaylistsSubChannelOutput
                {
                    YoutubePlaylistName = item.YoutubePlaylistName,
                    YoutubePlaylistId = item.YoutubePlaylistId,
                    AllyId = item.AllyId,
                    Highlighted = item.Highlighted,
                    Id = item.Id,
                    ImageUrl = item.ImageUrl,
                    Linked = item.Linked,
                    Tags = item.Tags
                });
            };

            Playlists = playlists;
        }
        public IEnumerable<YoutubeListPlaylistsSubChannelOutput> Playlists { get; set; }
    }

    public class YoutubeListPlaylistsSubChannelOutput : YoutubePlaylistListData
    {
        public List<YoutubePlaylistListData> SubChannels { get; set; } = new List<YoutubePlaylistListData>();
    }
}
