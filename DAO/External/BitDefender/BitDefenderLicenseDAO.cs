using DAO.Base;
using DAO.DBConnection;
using DAO.Hub.Order;
using DTO.External.BitDefender.Database;
using DTO.General.DAO.Output;
using DTO.Hub.BitDefender.Input;
using DTO.Hub.BitDefender.Output;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAO.External.BitDefender
{
    public class BitDefenderLicenseDAO : IBaseDAO<BitDefenderLicense>
    {
        private readonly HubOrderDAO HubOrderDAO;
        private readonly BitDefenderCategoryDAO BitDefenderCategoryDAO;
        internal RepositoryMongo<BitDefenderLicense> Repository;
        public BitDefenderLicenseDAO(IXDataDatabaseSettings settings)
        {
            HubOrderDAO = new(settings);
            BitDefenderCategoryDAO = new(settings);
            Repository = new(settings?.MongoDBSettings);
        }

        public DAOActionResultOutput Insert(BitDefenderLicense obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(BitDefenderLicense obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(BitDefenderLicense obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(BitDefenderLicense obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public BitDefenderLicense FindOne() => Repository.FindOne();

        public BitDefenderLicense FindOne(Expression<Func<BitDefenderLicense, bool>> predicate) => Repository.FindOne(predicate);

        public BitDefenderLicense FindById(string id) => Repository.FindById(id);

        public IEnumerable<BitDefenderLicense> Find(Expression<Func<BitDefenderLicense, bool>> predicate) => Repository.Collection.Find(Query<BitDefenderLicense>.Where(predicate));

        public IEnumerable<BitDefenderLicense> FindAll() => Repository.FindAll();

        public IEnumerable<BitDefenderLicense> GetAvailableLicenses(string categoryId, int quantity) => string.IsNullOrEmpty(categoryId) || quantity == 0 ? null : 
            Repository.Collection.Find(Query.And(Query<BitDefenderLicense>.EQ(x => x.BitDefenderCategoryId, categoryId), Query<BitDefenderLicense>.EQ(x => x.Used, false))).SetLimit(quantity);

        public IEnumerable<HubBitDefenderLicenseListData> List(HubBitDefenderLicensesListInput input)
        {
            var licenses = input == null ? FindAll() : input.Paginator == null ?
                Repository.Collection.Find(GenerateFilters(input.Filters)) :
                Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage);

            var orders = HubOrderDAO.List(new(new(licenses?.Select(x => x.OrderId))));
            var categories = BitDefenderCategoryDAO.FindAll();
            return licenses.Select(license => new HubBitDefenderLicenseListData(license)
            {
                CategoryName = categories?.FirstOrDefault(x => x.Id == license.BitDefenderCategoryId)?.Name,
                OrderCode = orders?.FirstOrDefault(x => x.Id == license.OrderId)?.Code ?? 0
            });
        }

        private IMongoQuery GenerateFilters(HubBitDefenderLicensesFiltersInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var queryList = new List<IMongoQuery>();
            if (!string.IsNullOrEmpty(input.AllyId))
                queryList.Add(Query<BitDefenderLicense>.EQ(x => x.AllyId, input.AllyId));

            if (!string.IsNullOrEmpty(input.CategoryId))
                queryList.Add(Query<BitDefenderLicense>.EQ(x => x.BitDefenderCategoryId, input.CategoryId));

            if (!string.IsNullOrEmpty(input.Key))
                queryList.Add(Query<BitDefenderLicense>.EQ(x => x.Key, input.Key));

            if (input.Used.HasValue)
                queryList.Add(Query<BitDefenderLicense>.EQ(x => x.Used, input.Used));

            if (input.OrderCode.HasValue)
                queryList.Add(Query<BitDefenderLicense>.EQ(x => x.OrderId, HubOrderDAO.FindOne(x => x.Code == input.OrderCode)?.Id));

            if (input.StartDate.HasValue && input.StartDate != DateTime.MinValue)
                queryList.Add(Query<BitDefenderLicense>.GTE(x => x.LastUpdate, input.StartDate.Value.Date));

            if (input.EndDate.HasValue && input.EndDate != DateTime.MinValue)
                queryList.Add(Query<BitDefenderLicense>.LTE(x => x.LastUpdate, input.EndDate.Value.Date.AddDays(1).AddMilliseconds(-1)));


            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }
    }
}
