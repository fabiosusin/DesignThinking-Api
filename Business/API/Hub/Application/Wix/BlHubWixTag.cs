using DAO.DBConnection;
using DAO.Hub.Application.Wix;
using DTO.General.Base.Api.Output;
using DTO.Hub.Application.Wix.Input;
using System.Linq;

namespace Business.API.Hub.Application.Wix
{
    public class BlHubWixTag
    {
        protected WixAllyTagDAO WixAllyTagDAO;

        public BlHubWixTag(XDataDatabaseSettings settings)
        {
            WixAllyTagDAO = new(settings);
        }

        public BaseApiOutput UpsertAllyTag(HubWixAllyTagInput input)
        {
            if (string.IsNullOrEmpty(input?.AllyId))
                return new("Aliado não informado");

            if (!(input.TagId?.Any() ?? false))
                return new("Nenhuma tag informada");

            return input.Linked ? WixAllyTagDAO.InsertAllyCategory(input) : WixAllyTagDAO.RemoveByTagId(input.TagId);
        }
    }
}
