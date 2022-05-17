using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.General.Faq.Input;
using DTO.Hub.Application.Faq.Database;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAO.Hub.Application.Faq
{
    public class AllyFaqDAO : IBaseDAO<AllyFaq>
    {
        internal RepositoryMongo<AllyFaq> Repository;
        public AllyFaqDAO(IXDataDatabaseSettings settings)
        {
            Repository = new(settings?.MongoDBSettings);
        }

        public DAOActionResultOutput Insert(AllyFaq obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(AllyFaq obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(AllyFaq obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(AllyFaq obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public AllyFaq FindOne() => Repository.FindOne();

        public AllyFaq FindOne(Expression<Func<AllyFaq, bool>> predicate) => Repository.FindOne(predicate);

        public AllyFaq FindById(string id) => Repository.FindById(id);

        public IEnumerable<AllyFaq> Find(Expression<Func<AllyFaq, bool>> predicate) => Repository.Collection.Find(Query<AllyFaq>.Where(predicate));

        public IEnumerable<AllyFaq> FindAll() => Repository.FindAll();

        public IEnumerable<AllyFaq> List(FaqListInput input) => input == null ?
            FindAll() :
            input.Paginator == null ?
                Repository.Collection.Find(GenerateFilters(input.Filters)) :
                Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage);
        
        private static IMongoQuery GenerateFilters(FaqFiltersInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var queryList = new List<IMongoQuery>();

            if (!string.IsNullOrEmpty(input.AllyId))
                queryList.Add(Query.Or(
                    Query<AllyFaq>.EQ(x => x.AllyId, input.AllyId),
                    Query.Or(Query<AllyFaq>.EQ(x => x.AllyId, null), Query<AllyFaq>.EQ(x => x.AllyId, string.Empty))));
            else
                Query.Or(Query<AllyFaq>.EQ(x => x.AllyId, null), Query<AllyFaq>.EQ(x => x.AllyId, string.Empty));

            if (input.Linked)
                queryList.Add(Query.And(Query<AllyFaq>.EQ(x => x.Linked, true)));

            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }
    }
}
