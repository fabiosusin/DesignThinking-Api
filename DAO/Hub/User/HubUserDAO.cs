using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Hub.User.Database;
using DTO.Hub.User.Input;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace DAO.Hub.UserDAO
{
    public class HubUserDAO : IBaseDAO<HubUser>
    {
        internal RepositoryMongo<HubUser> Repository;
        public HubUserDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(HubUser obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(HubUser obj)
        {
            if (string.IsNullOrEmpty(obj.Password))
                obj.Password = FindById(obj.Id)?.Password;

            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(HubUser obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(HubUser obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public HubUser FindOne() => Repository.FindOne();

        public HubUser FindOne(Expression<Func<HubUser, bool>> predicate) => Repository.FindOne(predicate);

        public HubUser FindById(string id) => Repository.FindById(id);

        public IEnumerable<HubUser> Find(Expression<Func<HubUser, bool>> predicate) => Repository.Collection.Find(Query<HubUser>.Where(predicate));

        public IEnumerable<HubUser> FindAll() => Repository.FindAll();

        public IEnumerable<HubUser> List(HubUserListInput input) => input == null ?
            FindAll() : input.Paginator == null ?
            Repository.Collection.Find(GenerateFilters(input.Filters)) : Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage);

        public IEnumerable<HubUser> List(HubUserListInput input, FieldsBuilder<HubUser> fields) => input == null ?
            Repository.Collection.FindAll().SetFields(fields) : input.Paginator == null ?
            Repository.Collection.Find(GenerateFilters(input.Filters)).SetFields(fields) : Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage).SetFields(fields);

        private static IMongoQuery GenerateFilters(HubUserFiltersInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var queryList = new List<IMongoQuery>();
            if (!string.IsNullOrEmpty(input.Name))
                queryList.Add(Query<HubUser>.Matches(x => x.Name, $"(?i).*{string.Join(".*", Regex.Split(input.Name, @"\s+").Select(x => Regex.Escape(x)))}.*"));

            if (!string.IsNullOrEmpty(input.Username))
                queryList.Add(Query<HubUser>.Matches(x => x.Username, $"(?i).*{string.Join(".*", Regex.Split(input.Username, @"\s+").Select(x => Regex.Escape(x)))}.*"));

            if (!string.IsNullOrEmpty(input.AllyId))
                queryList.Add(Query<HubUser>.EQ(x => x.AllyId, input.AllyId));

            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }
    }
}
