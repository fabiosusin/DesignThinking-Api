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
    public class HubProductPriceTableDAO : IBaseDAO<HubProductPriceTable>
    {
        internal RepositoryMongo<HubProductPriceTable> Repository;
        public HubProductPriceTableDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(HubProductPriceTable obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(HubProductPriceTable obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(HubProductPriceTable obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(HubProductPriceTable obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public HubProductPriceTable FindOne() => Repository.FindOne();

        public HubProductPriceTable FindOne(Expression<Func<HubProductPriceTable, bool>> predicate) => Repository.FindOne(predicate);

        public HubProductPriceTable FindById(string id) => Repository.FindById(id);

        public IEnumerable<HubProductPriceTable> Find(Expression<Func<HubProductPriceTable, bool>> predicate) => Repository.Collection.Find(Query<HubProductPriceTable>.Where(predicate));

        public IEnumerable<HubProductPriceTable> FindAll() => Repository.FindAll();
    }
}
