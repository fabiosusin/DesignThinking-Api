using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Intra.User.Database;
using DTO.Intra.User.Input;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace DAO.Intra.UserDAO
{
    public class IntraUserDAO : IBaseDAO<IntraUser>
    {
        internal RepositoryMongo<IntraUser> Repository;
        public IntraUserDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(IntraUser obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(IntraUser obj)
        {
            if (string.IsNullOrEmpty(obj.Password))
                obj.Password = FindById(obj.Id)?.Password;

            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(IntraUser obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(IntraUser obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public IntraUser FindOne() => Repository.FindOne();

        public IntraUser FindOne(Expression<Func<IntraUser, bool>> predicate) => Repository.FindOne(predicate);

        public IntraUser FindById(string id) => Repository.FindById(id);

        public IEnumerable<IntraUser> Find(Expression<Func<IntraUser, bool>> predicate) => Repository.Collection.Find(Query<IntraUser>.Where(predicate));

        public IEnumerable<IntraUser> FindAll() => Repository.FindAll();

        public IEnumerable<IntraUser> List(IntraUserListInput input) => input == null ?
            FindAll() : input.Paginator == null ?
            Repository.Collection.Find(GenerateFilters(input.Filters)) : Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage);

        public IEnumerable<IntraUser> List(IntraUserListInput input, FieldsBuilder<IntraUser> fields) => input == null ?
            Repository.Collection.FindAll().SetFields(fields) : input.Paginator == null ?
            Repository.Collection.Find(GenerateFilters(input.Filters)).SetFields(fields) : Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage).SetFields(fields);

        private static IMongoQuery GenerateFilters(IntraUserFiltersInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var queryList = new List<IMongoQuery>();
            if (!string.IsNullOrEmpty(input.Name))
                queryList.Add(Query<IntraUser>.Matches(x => x.Name, $"(?i).*{string.Join(".*", Regex.Split(input.Name, @"\s+").Select(x => Regex.Escape(x)))}.*"));

            if (!string.IsNullOrEmpty(input.Username))
                queryList.Add(Query<IntraUser>.Matches(x => x.Username, $"(?i).*{string.Join(".*", Regex.Split(input.Username, @"\s+").Select(x => Regex.Escape(x)))}.*"));

            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }
    }
}
