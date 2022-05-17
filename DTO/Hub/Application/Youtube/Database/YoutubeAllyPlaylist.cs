using DTO.General.Base.Database;

namespace DTO.Hub.Application.Youtube.Database
{
    public class YoutubeAllyPlaylist : BaseData
    {
        public YoutubeAllyPlaylist() { }
        public YoutubeAllyPlaylist(string allyId, string playlistId, bool highlighted)
        {
            AllyId = allyId;
            PlaylistId = playlistId;
            Highlighted = highlighted;
        }

        public string AllyId { get; set; }
        public string PlaylistId { get; set; }
        public bool Highlighted { get; set; }
    }
}
