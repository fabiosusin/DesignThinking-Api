using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Intra.EquipamentHistory.Input;
using DTO.Intra.LoanHistory.Database;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAO.Intra.LoanHistory
{
    public class IntraEquipmentHistoryDAO : IBaseDAO<IntraEquipmentHistory>
    {
        internal RepositoryMongo<IntraEquipmentHistory> Repository;

        public IntraEquipmentHistoryDAO(IXDataDatabaseSettings settings)
        {
            Repository = new(settings?.MongoDBSettings);
        }

        public DAOActionResultOutput Insert(IntraEquipmentHistory obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(IntraEquipmentHistory obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(IntraEquipmentHistory obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(IntraEquipmentHistory obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public IntraEquipmentHistory FindOne() => Repository.FindOne();

        public IntraEquipmentHistory FindOne(Expression<Func<IntraEquipmentHistory, bool>> predicate) => Repository.FindOne(predicate);

        public IntraEquipmentHistory FindById(string id) => Repository.FindById(id);

        public IEnumerable<IntraEquipmentHistory> Find(Expression<Func<IntraEquipmentHistory, bool>> predicate) => Repository.Collection.Find(Query<IntraEquipmentHistory>.Where(predicate));

        public IEnumerable<IntraEquipmentHistory> FindAll() => Repository.FindAll();

        public IEnumerable<IntraEquipmentHistory> List(IntraEquipmentHistoryListInput input) => input == null ?
            FindAll().OrderByDescending(x=> x.DevolutionDate) : input.Paginator == null ?
            Repository.Collection.Find(GenerateFilters(input.Filters)).SetSortOrder(SortBy<IntraEquipmentHistory>.Descending(x => x.DevolutionDate)) : 
            Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage).SetSortOrder(SortBy<IntraEquipmentHistory>.Descending(x => x.DevolutionDate));

        private static IMongoQuery GenerateFilters(IntraEquipmentHistoryFiltersInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var queryList = new List<IMongoQuery>();
            if (!string.IsNullOrEmpty(input.EquipmentId))
                queryList.Add(Query<IntraEquipmentHistory>.EQ(x => x.EquipmentId, input.EquipmentId));

            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }
    }
}
