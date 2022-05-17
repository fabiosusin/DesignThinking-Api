using DAO.DBConnection;
using DAO.Hub.Application.Database;
using DTO.Hub.Application.Sponsor.Input;
using DTO.Hub.Application.Sponsor.Output;
using System.Linq;

namespace Business.API.General.AppSponsor
{
    public class BlAppSponsor
    {
        protected AppSponsorDAO AppSponsorDAO;

        public BlAppSponsor(XDataDatabaseSettings settings)
        {
            AppSponsorDAO = new(settings);
        }

        public HubAppSponsorListOutput List(HubAppSponsorListInput input)
        {
            var result = AppSponsorDAO.List(input);
            if (!(result?.Any() ?? false))
                return new("Nenhum Patrocinador encontrada!");

            return new(result);
        }
    }
}
