using DAO.DBConnection;
using DAO.Hub.Product;
using DTO.Hub.Product.Database;
using DTO.Hub.Product.Input;
using DTO.Hub.Product.Output;
using System.Collections.Generic;
using System.Linq;

namespace Business.API.Hub.Product
{
    public class BlProduct
    {
        private readonly HubProductDAO HubProductDAO;
        private readonly HubCategoryDAO HubCategoryDAO;

        public BlProduct(XDataDatabaseSettings settings)
        {
            HubCategoryDAO = new(settings);
            HubProductDAO = new(settings);
        }

        public HubOrderProductListOutput OrderProductList(HubProductListInput input)
        {
            var result = HubProductDAO.OrderProductList(input);
            if (!(result?.Any() ?? false))
                return new("Nenhum Produto encontrado!");

            return new(result);
        }

        public HubProductListOutput List(HubProductListInput input)
        {
            var result = HubProductDAO.List(input);
            if (!(result?.Any() ?? false))
                return new("Nenhum Produto encontrado!");

            return new(result);
        }

        public IEnumerable<HubProductCategory> ProductCategoryList(string allyId) => HubCategoryDAO.GetAvailableCategories(allyId);

        public IEnumerable<HubProductCategory> GetAllProductCategories() => HubCategoryDAO.FindAll();
    }
}
