using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Hub.Ally.Database;
using DTO.Hub.Ally.Input;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using DTO.General.Log.Enum;
using DAO.General.Sequential;

namespace DAO.Hub.AllyDAO
{
    public class HubAllyDAO : IBaseDAO<HubAlly>
    {
        internal RepositoryMongo<HubAlly> Repository;
        protected SequentialCodeDAO SequencialCodeDAO;

        public HubAllyDAO(IXDataDatabaseSettings settings)
        {
            SequencialCodeDAO = new(settings);
            Repository = new(settings?.MongoDBSettings);
        }

        public DAOActionResultOutput Insert(HubAlly obj)
        {
            if (obj == null)
                return new("Objeto não informado");

            obj.Code = SequencialCodeDAO.GetNextCode(SequentialCodeTypeEnum.HubAlly);
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(HubAlly obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(HubAlly obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(HubAlly obj)
        {
            RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            var ally = FindById(id);
            if (ally == null)
                return new("Aliado não encontrado");

            SequencialCodeDAO.RollbackCode(SequentialCodeTypeEnum.HubAlly, ally.Code);
            Repository.RemoveById(id);
            return new(true);
        }

        public HubAlly FindOne() => Repository.FindOne();

        public HubAlly FindOne(Expression<Func<HubAlly, bool>> predicate) => Repository.FindOne(predicate);

        public HubAlly FindById(string id) => Repository.FindById(id);

        public IEnumerable<HubAlly> Find(Expression<Func<HubAlly, bool>> predicate) => Repository.Collection.Find(Query<HubAlly>.Where(predicate));

        public IEnumerable<HubAlly> FindAll() => Repository.FindAll();

        public decimal TotalAlly() => Repository.Collection.Count();

        public IEnumerable<HubAlly> List(HubAllyListInput input = null) => input == null ?
            FindAll().OrderBy(x => x.Code) : input.Paginator == null ?
            Repository.Collection.Find(GenerateFilters(input.Filters)).OrderBy(x=> x.Code) : Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage).OrderBy(x => x.Code);

        private static IMongoQuery GenerateFilters(HubAllyFiltersInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var queryList = new List<IMongoQuery>();
            if (!string.IsNullOrEmpty(input.Name))
                queryList.Add(Query<HubAlly>.Matches(x => x.Name, $"(?i).*{string.Join(".*", Regex.Split(input.Name, @"\s+").Select(x => Regex.Escape(x)))}.*"));

            if (!string.IsNullOrEmpty(input.CorporateName))
                queryList.Add(Query<HubAlly>.Matches(x => x.CorporateName, $"(?i).*{string.Join(".*", Regex.Split(input.CorporateName, @"\s+").Select(x => Regex.Escape(x)))}.*"));

            if (!string.IsNullOrEmpty(input.Cnpj))
                queryList.Add(Query<HubAlly>.EQ(x => x.Cnpj, input.Cnpj));

            if (input.Code.HasValue)
                queryList.Add(Query<HubAlly>.EQ(x => x.Code, input.Code.Value));

            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }
    }
}
