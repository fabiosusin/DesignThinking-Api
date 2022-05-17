using DAO.Base;
using DAO.DBConnection;
using DAO.Hub.Permission;
using DTO.General.DAO.Output;
using DTO.Hub.Product.Database;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAO.Hub.Product
{
    public class HubCategoryDAO : IBaseDAO<HubProductCategory>
    {
        internal RepositoryMongo<HubProductCategory> Repository;
        private readonly HubAllyPermissionDAO HubAllyPermissionDAO;
        public HubCategoryDAO(IXDataDatabaseSettings settings)
        {
            HubAllyPermissionDAO = new(settings);
            Repository = new(settings?.MongoDBSettings);
        }

        public DAOActionResultOutput Insert(HubProductCategory obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(HubProductCategory obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(HubProductCategory obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(HubProductCategory obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public HubProductCategory FindOne() => Repository.FindOne();

        public HubProductCategory FindOne(Expression<Func<HubProductCategory, bool>> predicate) => Repository.FindOne(predicate);

        public HubProductCategory FindById(string id) => Repository.FindById(id);

        public IEnumerable<HubProductCategory> Find(Expression<Func<HubProductCategory, bool>> predicate) => Repository.Collection.Find(Query<HubProductCategory>.Where(predicate));

        public IEnumerable<HubProductCategory> FindAll() => Repository.FindAll();

        public IEnumerable<HubProductCategory> GetAvailableCategories(string allyId)
        {
            var categoriesId = HubAllyPermissionDAO.FindOne(x => x.AllyId == allyId)?.ProductCategories.Where(x => x.Valid).Select(x => x.DataId);
            if (!(categoriesId?.Any() ?? false))
                return null;

            return Repository.Collection.Find(Query<HubProductCategory>.In(x => x.Id, categoriesId));
        }
    }
}
