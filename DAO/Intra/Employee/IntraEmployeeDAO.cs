using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Intra.Employee.Database;
using DTO.Intra.Employee.Input;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace DAO.Intra.Employee
{
    public class IntraEmployeeDAO : IBaseDAO<IntraEmployee>
    {
        internal RepositoryMongo<IntraEmployee> Repository;
        public IntraEmployeeDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(IntraEmployee obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(IntraEmployee obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(IntraEmployee obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(IntraEmployee obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public IntraEmployee FindOne() => Repository.FindOne();

        public IntraEmployee FindOne(Expression<Func<IntraEmployee, bool>> predicate) => Repository.FindOne(predicate);

        public IntraEmployee FindById(string id) => Repository.FindById(id);

        public IEnumerable<IntraEmployee> Find(Expression<Func<IntraEmployee, bool>> predicate) => Repository.Collection.Find(Query<IntraEmployee>.Where(predicate));

        public IEnumerable<IntraEmployee> FindAll() => Repository.FindAll();

        public long EmployeesCount() => Repository.Collection.Count();

        public IEnumerable<IntraEmployee> List(FieldsBuilder<IntraEmployee> fields) => Repository.Collection.FindAll().SetFields(fields);

        public IEnumerable<IntraEmployee> List(IntraEmployeeListInput input) => input == null ?
           FindAll() : input.Paginator == null ?
           Repository.Collection.Find(GenerateFilters(input.Filters)) : Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage);

        public IEnumerable<IntraEmployee> List(IntraEmployeeListInput input, FieldsBuilder<IntraEmployee> fields) => input == null ?
            Repository.Collection.FindAll().SetFields(fields) : input.Paginator == null ?
            Repository.Collection.Find(GenerateFilters(input.Filters)).SetFields(fields) : Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage).SetFields(fields);

        private static IMongoQuery GenerateFilters(IntraEmployeeFiltersInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var queryList = new List<IMongoQuery>();
            if (!string.IsNullOrEmpty(input.Name))
                queryList.Add(Query<IntraEmployee>.Matches(x => x.Name, $"(?i).*{string.Join(".*", Regex.Split(input.Name, @"\s+").Select(x => Regex.Escape(x)))}.*"));

            if (!string.IsNullOrEmpty(input.CpfCnpj))
                queryList.Add(Query<IntraEmployee>.Matches(x => x.CpfCnpj, $"(?i).*{string.Join(".*", Regex.Split(input.CpfCnpj, @"\s+").Select(x => Regex.Escape(x)))}.*"));

            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }
    }
}
