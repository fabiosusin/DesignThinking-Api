using DAO.Base;
using DAO.DBConnection;
using DAO.Hub.AllyDAO;
using DTO.General.DAO.Output;
using DTO.Hub.Application.Wix.Database;
using DTO.Hub.Application.Wix.Input;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAO.Hub.Application.Wix
{
    public class WixAllyCategoryDAO : IBaseDAO<WixAllyCategory>
    {
        protected HubAllyDAO HubAllyDAO;
        internal RepositoryMongo<WixAllyCategory> Repository;
        public WixAllyCategoryDAO(IXDataDatabaseSettings settings)
        {
            HubAllyDAO = new(settings);
            Repository = new(settings?.MongoDBSettings);
        }

        public DAOActionResultOutput InsertCategoryMasterAlly(string categoryId)
        {
            var masterAlly = HubAllyDAO.FindOne(x => x.IsMasterAlly);
            return InsertAllyCategory(new HubWixAllyCategoryInput(masterAlly?.Id, categoryId));
        }

        public DAOActionResultOutput InsertAllyCategory(HubWixAllyCategoryInput input)
        {
            if (string.IsNullOrEmpty(input?.AllyId))
                return new("Aliado não informado");

            if (!(input.CategoryId?.Any() ?? false))
                return new("Nenhuma categoria informada");

            var category = Repository.Collection.FindOne(Query.And(Query<WixAllyCategory>.EQ(x => x.CategoryId, input.CategoryId), Query<WixAllyCategory>.EQ(x => x.AllyId, input.AllyId)));
            if (category == null)
                Insert(new WixAllyCategory(input.AllyId, input.CategoryId));

            return new(true);
        }

        public DAOActionResultOutput Insert(WixAllyCategory obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(WixAllyCategory obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(WixAllyCategory obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(WixAllyCategory obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveByCategoryId(string categoryId)
        {
            if (string.IsNullOrEmpty(categoryId))
                return new("Id da Categoria não informada");

            var category = Repository.Collection.FindOne(Query<WixAllyCategory>.EQ(x => x.CategoryId, categoryId));
            if (category == null)
                return new("Categoria não encontrada!");

            Remove(category);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public WixAllyCategory FindOne() => Repository.FindOne();

        public WixAllyCategory FindOne(Expression<Func<WixAllyCategory, bool>> predicate) => Repository.FindOne(predicate);

        public WixAllyCategory FindById(string id) => Repository.FindById(id);

        public IEnumerable<WixAllyCategory> Find(Expression<Func<WixAllyCategory, bool>> predicate) => Repository.Collection.Find(Query<WixAllyCategory>.Where(predicate));

        public IEnumerable<WixAllyCategory> FindAll() => Repository.FindAll();

        public IEnumerable<string> GetAllyCategoriesId(string allyId) => Repository.Collection.Find(Query<WixAllyCategory>.EQ(x => x.AllyId, allyId)).SetFields(Fields<WixAllyCategory>.Include(x => x.CategoryId))?.Select(x => x.CategoryId);
    }
}
