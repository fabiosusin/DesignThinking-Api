using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Hub.Permission.Database;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAO.Hub.Permission
{
    public class HubAllyPermissionDAO : IBaseDAO<HubAllyPermission>
    {
        internal RepositoryMongo<HubAllyPermission> Repository;
        public HubAllyPermissionDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(HubAllyPermission obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(HubAllyPermission obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(HubAllyPermission obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(HubAllyPermission obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public HubAllyPermission FindOne() => Repository.FindOne();

        public HubAllyPermission FindOne(Expression<Func<HubAllyPermission, bool>> predicate) => Repository.FindOne(predicate);

        public HubAllyPermission FindById(string id) => Repository.FindById(id);

        public IEnumerable<HubAllyPermission> Find(Expression<Func<HubAllyPermission, bool>> predicate) => Repository.Collection.Find(Query<HubAllyPermission>.Where(predicate));

        public IEnumerable<HubAllyPermission> FindAll() => Repository.FindAll();
    }
}
