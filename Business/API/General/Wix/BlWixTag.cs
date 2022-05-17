using DAO.DBConnection;
using DAO.External.Wix;
using DTO.External.Wix.Input;
using DTO.External.Wix.Output;
using System.Linq;

namespace Business.API.General.Wix
{
    public class BlWixTag
    {
        protected WixTagDAO WixTagDAO;

        public BlWixTag(XDataDatabaseSettings settings)
        {
            WixTagDAO = new(settings);
        }

        public WixTagListOutput List(WixTagListInput input)
        {
            var result = WixTagDAO.List(input);
            if (!(result?.Any() ?? false))
                return new("Nenhuma Categoria encontrada!");

            return new(result);
        }

    }
}
