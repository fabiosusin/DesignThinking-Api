using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.General.Surf.Database;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAO.General.Surf
{
    public class SurfCustomerMsisdnDAO : IBaseDAO<SurfCustomerMsisdn>
    {
        internal RepositoryMongo<SurfCustomerMsisdn> Repository;
        public SurfCustomerMsisdnDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(SurfCustomerMsisdn obj)
        {
            if (string.IsNullOrEmpty(obj.HubCustomerId))
                return new("HubCustomerId não informado!");

            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(SurfCustomerMsisdn obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(SurfCustomerMsisdn obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(SurfCustomerMsisdn obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public SurfCustomerMsisdn FindOne() => Repository.FindOne();

        public SurfCustomerMsisdn FindOne(Expression<Func<SurfCustomerMsisdn, bool>> predicate) => Repository.FindOne(predicate);

        public SurfCustomerMsisdn FindById(string id) => Repository.FindById(id);

        public IEnumerable<SurfCustomerMsisdn> Find(Expression<Func<SurfCustomerMsisdn, bool>> predicate) => Repository.Collection.Find(Query<SurfCustomerMsisdn>.Where(predicate));

        public IEnumerable<SurfCustomerMsisdn> FindAll() => Repository.FindAll();
    }
}
