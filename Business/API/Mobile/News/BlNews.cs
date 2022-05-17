using DAO.DBConnection;
using DAO.External.Wix;
using DTO.Mobile.News.Input;
using DTO.Mobile.News.Output;
using System.Linq;

namespace Business.API.Mobile.News
{
    public class BlNews
    {
        protected WixPostDAO WixPostDAO;
        protected WixCategoryDAO WixCategoryDAO;
        public BlNews(XDataDatabaseSettings settings)
        {
            WixPostDAO = new(settings);
            WixCategoryDAO = new(settings);
        }

        public AppNewsListOutput GetNewsList(AppNewsListInput input)
        {
            var data = WixPostDAO.List(input);
            return (data?.Any() ?? false) ? new(data) : new("Nenhuma Notícia encontrada!");
        }

        public AppNewsDetailsOutput GetNewsDetails(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new("Id da notícia não informado");

            var data = WixPostDAO.FindNewsOutputById(id);
            return data == null ? new("Nenhuma Notícia encontrada!") : new(data);
        }

    }
}
