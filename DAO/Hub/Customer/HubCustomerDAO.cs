using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Hub.Customer.Database;
using DTO.Hub.Customer.Input;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace DAO.Hub.CustomerDAO
{
    public class HubCustomerDAO : IBaseDAO<HubCustomer>
    {
        internal RepositoryMongo<HubCustomer> Repository;
        public HubCustomerDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(HubCustomer obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput UpdateAsaasId(string objId, string asaasId)
        {
            Repository.Collection.Update(Query<HubCustomer>.EQ(x => x.Id, objId), Update<HubCustomer>.Set(x => x.AsaasId, asaasId));
            return new(true);
        }

        public DAOActionResultOutput Update(HubCustomer obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(HubCustomer obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(HubCustomer obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public HubCustomer FindOne() => Repository.FindOne();

        public HubCustomer FindOne(Expression<Func<HubCustomer, bool>> predicate) => Repository.FindOne(predicate);

        public HubCustomer FindById(string id) => Repository.FindById(id);

        public IEnumerable<HubCustomer> Find(Expression<Func<HubCustomer, bool>> predicate) => Repository.Collection.Find(Query<HubCustomer>.Where(predicate));

        public IEnumerable<HubCustomer> FindAll() => Repository.FindAll();

        public decimal TotalCustomer(string allyId = null) => string.IsNullOrEmpty(allyId) ? Repository.Collection.Count() : Repository.Collection.Count(Query<HubCustomer>.EQ(x => x.AllyId, allyId));

        public IEnumerable<HubCustomer> List(HubCustomerListInput input) => input == null ?
           FindAll() : input.Paginator == null ?
           Repository.Collection.Find(GenerateFilters(input.Filters)) : Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage);

        public IEnumerable<HubCustomer> List(HubCustomerListInput input, FieldsBuilder<HubCustomer> fields) => input == null ?
            Repository.Collection.FindAll().SetFields(fields) : input.Paginator == null ?
            Repository.Collection.Find(GenerateFilters(input.Filters)).SetFields(fields) : Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage).SetFields(fields);

        private static IMongoQuery GenerateFilters(HubCustomerFiltersInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var queryList = new List<IMongoQuery>();
            if (input.Ids?.Any() ?? false)
                queryList.Add(Query<HubCustomer>.In(x => x.Id, input.Ids));

            if (!string.IsNullOrEmpty(input.Name))
                queryList.Add(Query<HubCustomer>.Matches(x => x.Name, $"(?i).*{string.Join(".*", Regex.Split(input.Name, @"\s+").Select(x => Regex.Escape(x)))}.*"));

            if (!string.IsNullOrEmpty(input.Document))
                queryList.Add(Query<HubCustomer>.Matches(x => x.Document.Data, $"(?i).*{string.Join(".*", Regex.Split(input.Document, @"\s+").Select(x => Regex.Escape(x)))}.*"));

            if (!string.IsNullOrEmpty(input.Rg))
                queryList.Add(Query<HubCustomer>.Matches(x => x.Rg.Number, $"(?i).*{string.Join(".*", Regex.Split(input.Rg, @"\s+").Select(x => Regex.Escape(x)))}.*"));

            if (!string.IsNullOrEmpty(input.AllyId))
                queryList.Add(Query<HubCustomer>.EQ(x => x.AllyId, input.AllyId));

            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }
    }
}
