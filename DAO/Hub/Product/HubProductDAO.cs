using DAO.Base;
using DAO.DBConnection;
using DAO.External.BitDefender;
using DAO.Hub.Permission;
using DAO.Mobile.Surf;
using DTO.General.DAO.Output;
using DTO.Hub.Product.Database;
using DTO.Hub.Product.Input;
using DTO.Hub.Product.Output;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace DAO.Hub.Product
{
    public class HubProductDAO : IBaseDAO<HubProduct>
    {
        private readonly HubPriceTableDAO HubPriceTableDAO;
        private readonly SurfMobilePlanDAO SurfMobilePlanDAO;
        private readonly HubAllyPermissionDAO HubAllyPermissionDAO;
        private readonly HubProductPriceTableDAO HubProductTablePriceDAO;
        internal RepositoryMongo<HubProduct> Repository;
        public HubProductDAO(IXDataDatabaseSettings settings)
        {
            HubPriceTableDAO = new(settings);
            SurfMobilePlanDAO = new(settings);
            HubAllyPermissionDAO = new(settings);
            HubProductTablePriceDAO = new(settings);
            Repository = new(settings?.MongoDBSettings);
        }

        public DAOActionResultOutput Insert(HubProduct obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(HubProduct obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(HubProduct obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(HubProduct obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public HubProduct FindOne() => Repository.FindOne();

        public HubProduct FindOne(Expression<Func<HubProduct, bool>> predicate) => Repository.FindOne(predicate);

        public HubProduct FindById(string id) => Repository.FindById(id);

        public IEnumerable<HubProduct> Find(Expression<Func<HubProduct, bool>> predicate) => Repository.Collection.Find(Query<HubProduct>.Where(predicate));

        public IEnumerable<HubProduct> FindAll() => Repository.FindAll();

        public IEnumerable<HubProduct> GetProductsOrderDetails(IEnumerable<string> ids) => FindProducts(ids, Fields<HubProduct>.Include(x => x.Name).Include(x => x.Code).Include(x => x.BitDefenderCategoryId).Include(x => x.SurfMobilePlanId).Include(x => x.CategoryId));

        private IEnumerable<HubProduct> FindProducts(IEnumerable<string> ids, IMongoFields fields) => ids?.Any() ?? false ? Repository.Collection.Find(Query<HubProduct>.In(x => x.Id, ids)).SetFields(fields) : null;

        public IEnumerable<HubProductList> OrderProductList(HubProductListInput input)
        {
            var products = List(input);
            if (!(products?.Any() ?? false))
                return null;

            var plans = SurfMobilePlanDAO.FindAll();
            var result = new List<HubProductList>();
            foreach (var product in products)
            {
                if (product.Price == null)
                    continue;

                var newItem = new HubProductList(product);
                newItem.MobilePlan = plans.FirstOrDefault(x => x.Id == newItem.SurfMobilePlanId);
                result.Add(newItem);
            }

            return result;
        }

        public IEnumerable<HubProduct> List(HubProductListInput input)
        {
            var products = new List<HubProduct>();
            if (input == null)
                products = FindAll().ToList();
            else if (input.Paginator == null)
                products = Repository.Collection.Find(GenerateFilters(input.Filters)).ToList();
            else
                products = Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage).ToList();

            var allyTablePrice = !string.IsNullOrEmpty(input?.Filters?.AllyId) ? HubPriceTableDAO.FindOne(x => x.AllyId == input.Filters.AllyId) : null;
            if (allyTablePrice != null && (products?.Any() ?? false))
            {
                var productsIds = products.Select(x => x.Id);
                var productPrices = HubProductTablePriceDAO.Find(x => x.TablePriceId == allyTablePrice.Id && productsIds.Contains(x.ProductId));
                if (productPrices?.Any() ?? false)
                {
                    foreach (var product in products)
                    {
                        var price = productPrices.FirstOrDefault(x => x.ProductId == product.Id);
                        if (price == null)
                            continue;

                        product.Price = price.Price;
                    }
                }
            }

            return products;
        }

        private IMongoQuery GenerateFilters(HubProductFiltersInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var queryList = new List<IMongoQuery>();
            if (!string.IsNullOrEmpty(input.Name))
                queryList.Add(Query<HubProduct>.Matches(x => x.Name, $"(?i).*{string.Join(".*", Regex.Split(input.Name, @"\s+").Select(x => Regex.Escape(x)))}.*"));

            if (!string.IsNullOrEmpty(input.Code))
                queryList.Add(Query<HubProduct>.Matches(x => x.Code, $"(?i).*{string.Join(".*", Regex.Split(input.Code, @"\s+").Select(x => Regex.Escape(x)))}.*"));

            if (!string.IsNullOrEmpty(input.CategoryId))
                queryList.Add(Query<HubProduct>.EQ(x => x.CategoryId, input.CategoryId));

            if (!string.IsNullOrEmpty(input.AllyId))
            {
                queryList.Add(Query<HubProduct>.EQ(x => x.AllyId, input.AllyId));
                queryList.Add(Query<HubProduct>.In(x => x.CategoryId, HubAllyPermissionDAO.FindOne(x => x.AllyId == input.AllyId)?.ProductCategories.Where(x => x.Valid).Select(x => x.DataId) ?? new List<string>()));
            }

            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }

    }
}
