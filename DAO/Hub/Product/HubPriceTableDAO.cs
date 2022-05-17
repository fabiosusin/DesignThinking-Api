using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Hub.Product.Database;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAO.Hub.Product
{
    public class HubPriceTableDAO : IBaseDAO<HubPriceTable>
    {
        internal RepositoryMongo<HubPriceTable> Repository;
        public HubPriceTableDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(HubPriceTable obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(HubPriceTable obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(HubPriceTable obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(HubPriceTable obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public HubPriceTable FindOne() => Repository.FindOne();

        public HubPriceTable FindOne(Expression<Func<HubPriceTable, bool>> predicate) => Repository.FindOne(predicate);

        public HubPriceTable FindById(string id) => Repository.FindById(id);

        public IEnumerable<HubPriceTable> Find(Expression<Func<HubPriceTable, bool>> predicate) => Repository.Collection.Find(Query<HubPriceTable>.Where(predicate));

        public IEnumerable<HubPriceTable> FindAll() => Repository.FindAll();
    }
}
