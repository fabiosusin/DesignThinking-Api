using DAO.DBConnection;
using DAO.Integration.Youtube;
using DTO.Hub.Application.Youtube.Input;
using DTO.Hub.Application.Youtube.Output;
using System.Linq;

namespace Business.API.General.Youtube
{
    public class BlYoutubeChannels
    {
        private readonly YoutubeChannelsDAO YoutubeChannelsDAO;

        public BlYoutubeChannels(XDataDatabaseSettings settings)
        {
            YoutubeChannelsDAO = new(settings);
        }

        public HubYoutubeChannelsListOutput List(HubYoutubeListInput input)
        {
            var result = YoutubeChannelsDAO.List(input);
            if (!(result?.Any() ?? false))
                return new("Nenhum canal encontrado!");

            return new(result);
        }
    }
}
