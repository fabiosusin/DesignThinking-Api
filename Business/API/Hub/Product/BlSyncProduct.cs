using Business.API.Hub.Integration.Sige.Product;
using DAO.DBConnection;
using DAO.Hub.Product;
using DTO.General.Base.Api.Output;
using DTO.Integration.Sige.Product.Output;
using DTO.Hub.Product.Database;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAO.Hub.AllyDAO;
using DTO.Hub.Ally.Database;
using Newtonsoft.Json;

namespace Business.API.Hub.Product
{
    public class BlSyncProduct
    {
        private readonly HubAllyDAO HubAllyDAO;
        private readonly HubProductDAO ProductDAO;
        private readonly HubCategoryDAO CategoryDAO;
        private readonly BlSigeProduct BlSigeProduct;

        public BlSyncProduct(XDataDatabaseSettings settings)
        {
            HubAllyDAO = new(settings);
            ProductDAO = new(settings);
            CategoryDAO = new(settings);
            BlSigeProduct = new(settings);
        }

        public async Task<BaseApiOutput> GetProducts()
        {
            var products = new List<SigeProductInput>();
            IEnumerable<HubAlly> allys = null;

            try
            {
                products = (await BlSigeProduct.GetProducts().ConfigureAwait(false)).ToList();
                allys = HubAllyDAO.List();
            }
            catch { return new("Não foi possível realizar a requisição"); }

            foreach (var item in products)
            {
                var category = CategoryDAO.FindOne(x => x.Name == item.Categoria);
                if (category == null && !string.IsNullOrEmpty(item.Categoria))
                {
                    var categoryInsertResult = CategoryDAO.Insert(new HubProductCategory(item.Categoria));
                    if (categoryInsertResult.Success)
                        category = (HubProductCategory)categoryInsertResult.Data;
                }

                var newItem = new HubProduct(item)
                {
                    CategoryId = category?.Id
                };

                foreach (var ally in allys)
                {
                    // Perde a referência de memória e não substitui o objeto original
                    var product = JsonConvert.DeserializeObject<HubProduct>(JsonConvert.SerializeObject(newItem));
                    product.AllyId = ally.Id;


                    var existing = ProductDAO.FindOne(x => x.SigeId == product.SigeId && x.AllyId == ally.Id);
                    if (existing != null)
                    {
                        product.Id = existing.Id;
                        product.Name = existing.Name;
                        product.BitDefenderCategoryId = existing.BitDefenderCategoryId;
                        product.SurfMobilePlanId = existing.SurfMobilePlanId;
                        ProductDAO.Update(product);
                    }
                    else
                    {
                        if (!ally.IsMasterAlly)
                            product.Name += $" {ally.Name}";

                        ProductDAO.Insert(product);
                    }
                }
            }

            return new(true);
        }
    }
}
