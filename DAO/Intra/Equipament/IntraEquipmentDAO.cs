using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Intra.Equipament.Database;
using DTO.Intra.Equipament.Input;
using DTO.Intra.Loan.Output;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace DAO.Intra.EquipamentDAO
{
    public class IntraEquipmentDAO : IBaseDAO<IntraEquipment>
    {
        internal RepositoryMongo<IntraEquipment> Repository;
        public IntraEquipmentDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(IntraEquipment obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(IntraEquipment obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(IntraEquipment obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(IntraEquipment obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public IntraEquipment FindOne() => Repository.FindOne();

        public IntraEquipment FindOne(Expression<Func<IntraEquipment, bool>> predicate) => Repository.FindOne(predicate);

        public IntraEquipment FindById(string id) => Repository.FindById(id);

        public IEnumerable<IntraEquipment> Find(Expression<Func<IntraEquipment, bool>> predicate) => Repository.Collection.Find(Query<IntraEquipment>.Where(predicate));

        public IEnumerable<IntraEquipment> FindAll() => Repository.FindAll();

        public DAOActionResultOutput UpdateReturnedEquipment(List<IntraEquipmentDetails> equipments)
        {
            if (!(equipments?.Any() ?? false))
                return new("Equipamentos não informados!");

            equipments.ForEach(x => Repository.Collection.Update(Query<IntraEquipment>.EQ(y => y.Id, x.Id), Update<IntraEquipment>.Set(y => y.DamageNote, x.DamageNote).Set(y => y.Loaned, false)));
            return new(true);
        }

        public long EquipmentsCount() => Repository.Collection.Count();

        public long EquipmentsLoanedCount() => Repository.Collection.Count(Query<IntraEquipment>.EQ(x=> x.Loaned, true));

        public IEnumerable<IntraEquipment> ListWithFields(FieldsBuilder<IntraEquipment> fields) => Repository.Collection.FindAll().SetFields(fields);

        public IEnumerable<IntraEquipment> List(IntraEquipmentListInput input) => input == null ?
            FindAll() : input.Paginator == null ?
            Repository.Collection.Find(GenerateFilters(input.Filters)) : Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage);

        private static IMongoQuery GenerateFilters(IntraEquipmentFiltersInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var queryList = new List<IMongoQuery>();
            if (input.Ids?.Any() ?? false)
                queryList.Add(Query<IntraEquipment>.In(x => x.Id, input.Ids));

            if (!string.IsNullOrEmpty(input.Name))
                queryList.Add(Query<IntraEquipment>.Matches(x => x.Name, $"(?i).*{string.Join(".*", Regex.Split(input.Name, @"\s+").Select(x => Regex.Escape(x)))}.*"));

            if (input.Loaned.HasValue)
                queryList.Add(Query<IntraEquipment>.EQ(x => x.Loaned, input.Loaned.Value));

            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }
    }
}
