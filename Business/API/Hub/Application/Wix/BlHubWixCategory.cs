using DAO.DBConnection;
using DAO.Hub.Application.Wix;
using DTO.General.Base.Api.Output;
using DTO.Hub.Application.Wix.Input;
using System.Linq;

namespace Business.API.Hub.Application.Wix
{
    public class BlHubWixCategory
    {
        protected WixAllyCategoryDAO WixAllyCategoryDAO;

        public BlHubWixCategory(XDataDatabaseSettings settings)
        {
            WixAllyCategoryDAO = new(settings);
        }

        public BaseApiOutput UpsertAllyCategory(HubWixAllyCategoryInput input)
        {
            if (string.IsNullOrEmpty(input?.AllyId))
                return new("Aliado não informado");

            if (!(input.CategoryId?.Any() ?? false))
                return new("Nenhuma categoria informada");

            return input.Linked ? WixAllyCategoryDAO.InsertAllyCategory(input) : WixAllyCategoryDAO.RemoveByCategoryId(input.CategoryId);
        }
    }
}
