using DAO.Base;
using DAO.DBConnection;
using DAO.Hub.AllyDAO;
using DTO.General.DAO.Output;
using DTO.Hub.AccountPlan.Database;
using DTO.Hub.AccountPlan.Input;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace DAO.Hub.AccountPlan
{
    public class HubAccountPlanDAO : IBaseDAO<HubAccountPlan>
    {
        protected HubAllyDAO HubAllyDAO;
        internal RepositoryMongo<HubAccountPlan> Repository;
        public HubAccountPlanDAO(IXDataDatabaseSettings settings)
        {
            HubAllyDAO = new(settings);
            Repository = new(settings?.MongoDBSettings);
        }

        public DAOActionResultOutput Insert(HubAccountPlan obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(HubAccountPlan obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(HubAccountPlan obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(HubAccountPlan obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public HubAccountPlan FindOne() => Repository.FindOne();

        public HubAccountPlan FindOne(Expression<Func<HubAccountPlan, bool>> predicate) => Repository.FindOne(predicate);

        public HubAccountPlan FindById(string id) => Repository.FindById(id);

        public IEnumerable<HubAccountPlan> Find(Expression<Func<HubAccountPlan, bool>> predicate) => Repository.Collection.Find(Query<HubAccountPlan>.Where(predicate));

        public IEnumerable<HubAccountPlan> FindAll() => Repository.FindAll();

        public IEnumerable<HubAccountPlan> List(HubAccountPlanListInput input) => input == null ?
            FindAll() : input.Paginator == null ?
            Repository.Collection.Find(GenerateFilters(input.Filters)) : Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage);

        public IEnumerable<HubAccountPlan> List(HubAccountPlanListInput input, FieldsBuilder<HubAccountPlan> fields) => input == null ?
            Repository.Collection.FindAll().SetFields(fields) : input.Paginator == null ?
            Repository.Collection.Find(GenerateFilters(input.Filters)).SetFields(fields) : Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage).SetFields(fields);

        public HubAccountPlan GetAllyAccountPlan(string allyId)
        {
            if (string.IsNullOrEmpty(allyId))
                return null;

            var ally = HubAllyDAO.FindById(allyId);
            if (string.IsNullOrEmpty(ally?.AccountPlanId))
                return null;

            return FindById(ally.AccountPlanId);
        }

        private static IMongoQuery GenerateFilters(HubAccountPlanFiltersInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            return !string.IsNullOrEmpty(input?.Name) ? Query.And(Query<HubAccountPlan>.Matches(x => x.Name, $"(?i).*{string.Join(".*", Regex.Split(input.Name, @"\s+").Select(x => Regex.Escape(x)))}.*")) : emptyResult;
        }
    }
}
