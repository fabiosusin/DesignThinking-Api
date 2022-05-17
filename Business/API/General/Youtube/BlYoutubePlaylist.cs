using DAO.DBConnection;
using DAO.Hub.Application.Youtube;
using DAO.Integration.Youtube;
using DTO.Hub.Application.Youtube.Input;
using DTO.Hub.Application.Youtube.Output;
using System.Linq;

namespace Business.API.General.Youtube
{
    public class BlYoutubePlaylist
    {
        private readonly YoutubePlaylistDAO YoutubePlaylistDAO;

        public BlYoutubePlaylist(XDataDatabaseSettings settings)
        {
            YoutubePlaylistDAO = new(settings);
        }

        public HubYoutubePlaylistListOutput List(HubYoutubeListInput input)
        {
            var result = YoutubePlaylistDAO.List(input);
            if (!(result?.Any() ?? false))
                return new("Nenhuma Playlist encontrada!");

            return new(result);
        }
    }
}
