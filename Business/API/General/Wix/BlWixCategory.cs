using DAO.DBConnection;
using DAO.External.Wix;
using DTO.External.Wix.Input;
using DTO.External.Wix.Output;
using DTO.Mobile.News.Output;
using System.Linq;

namespace Business.API.General.Wix
{
    public class BlWixCategory
    {
        protected WixCategoryDAO WixCategoryDAO;

        public BlWixCategory(XDataDatabaseSettings settings)
        {
            WixCategoryDAO = new(settings);
        }

        public AppCategoryDetailsOutput GetCategoryDetailsByWixId(string id) => string.IsNullOrEmpty(id) ? new("Id da categoria não informado") : new(WixCategoryDAO.FindOne(x => x.WixCategoryId == id));

        public WixCategoryListOutput List(WixCategoryListInput input)
        {
            var result = WixCategoryDAO.List(input);
            if (!(result?.Any() ?? false))
                return new("Nenhuma Categoria encontrada!");

            return new(result);
        }

    }
}
