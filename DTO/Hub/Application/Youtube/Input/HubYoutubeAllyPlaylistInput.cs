namespace DTO.Hub.Application.Youtube.Input
{
    public class HubYoutubeAllyPlaylistInput
    {
        public HubYoutubeAllyPlaylistInput() { }
        public HubYoutubeAllyPlaylistInput(string allyId, string playlistId)
        {
            AllyId = allyId;
            PlaylistId = playlistId;
        }

        public string AllyId { get; set; }
        public string PlaylistId { get; set; }
        public bool Highlighted { get; set; }
        public bool Linked { get; set; }
    }
}
