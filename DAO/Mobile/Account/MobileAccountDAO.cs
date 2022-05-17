using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Mobile.Account.Database;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAO.Mobile.Account
{
    public class MobileAccountDAO : IBaseDAO<AppCustomerAccount>
    {
        internal RepositoryMongo<AppCustomerAccount> Repository;
        public MobileAccountDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(AppCustomerAccount obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(AppCustomerAccount obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(AppCustomerAccount obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(AppCustomerAccount obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public AppCustomerAccount FindOne() => Repository.FindOne();

        public AppCustomerAccount FindOne(Expression<Func<AppCustomerAccount, bool>> predicate) => Repository.FindOne(predicate);

        public AppCustomerAccount FindById(string id) => Repository.FindById(id);

        public IEnumerable<AppCustomerAccount> Find(Expression<Func<AppCustomerAccount, bool>> predicate) => Repository.Collection.Find(Query<AppCustomerAccount>.Where(predicate));

        public IEnumerable<AppCustomerAccount> FindAll() => Repository.FindAll();
    }
}
