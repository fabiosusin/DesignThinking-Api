using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Hub.Order.Database;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAO.Hub.Order
{
    public class HubProductOrderDAO : IBaseDAO<HubProductOrder>
    {
        internal RepositoryMongo<HubProductOrder> Repository;
        public HubProductOrderDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(HubProductOrder obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(HubProductOrder obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(HubProductOrder obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(HubProductOrder obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public HubProductOrder FindOne() => Repository.FindOne();

        public HubProductOrder FindOne(Expression<Func<HubProductOrder, bool>> predicate) => Repository.FindOne(predicate);

        public HubProductOrder FindById(string id) => Repository.FindById(id);

        public IEnumerable<HubProductOrder> Find(Expression<Func<HubProductOrder, bool>> predicate) => Repository.Collection.Find(Query<HubProductOrder>.Where(predicate));

        public IEnumerable<HubProductOrder> FindAll() => Repository.FindAll();

        public IEnumerable<HubProductOrder> GetProductsOrder(string orderId) => string.IsNullOrEmpty(orderId) ? null : Find(x => x.OrderId == orderId);

        public void RemoveProductsOrder(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
                return;

            Repository.Collection.Remove(Query<HubProductOrder>.EQ(x => x.OrderId, orderId));
        }
    }
}
