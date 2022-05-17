using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Hub.Permission.Database;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAO.Hub.Permission
{
    public class HubUserPermissionDAO : IBaseDAO<HubUserPermission>
    {
        internal RepositoryMongo<HubUserPermission> Repository;
        public HubUserPermissionDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(HubUserPermission obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(HubUserPermission obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(HubUserPermission obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(HubUserPermission obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public HubUserPermission FindOne() => Repository.FindOne();

        public HubUserPermission FindOne(Expression<Func<HubUserPermission, bool>> predicate) => Repository.FindOne(predicate);

        public HubUserPermission FindById(string id) => Repository.FindById(id);

        public IEnumerable<HubUserPermission> Find(Expression<Func<HubUserPermission, bool>> predicate) => Repository.Collection.Find(Query<HubUserPermission>.Where(predicate));

        public IEnumerable<HubUserPermission> FindAll() => Repository.FindAll();
    }
}
