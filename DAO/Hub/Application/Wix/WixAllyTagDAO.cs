using DAO.Base;
using DAO.DBConnection;
using DAO.Hub.AllyDAO;
using DTO.General.DAO.Output;
using DTO.Hub.Application.Wix.Database;
using DTO.Hub.Application.Wix.Input;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAO.Hub.Application.Wix
{
    public class WixAllyTagDAO : IBaseDAO<WixAllyTag>
    {
        protected HubAllyDAO HubAllyDAO;
        internal RepositoryMongo<WixAllyTag> Repository;
        public WixAllyTagDAO(IXDataDatabaseSettings settings)
        {
            HubAllyDAO = new(settings);
            Repository = new(settings?.MongoDBSettings);
        }

        public DAOActionResultOutput InsertCategoryMasterAlly(string categoryId)
        {
            var masterAlly = HubAllyDAO.FindOne(x => x.IsMasterAlly);
            return InsertAllyCategory(new HubWixAllyTagInput(masterAlly?.Id, categoryId));
        }

        public DAOActionResultOutput InsertAllyCategory(HubWixAllyTagInput input)
        {
            if (string.IsNullOrEmpty(input?.AllyId))
                return new("Aliado não informado");

            if (!(input.TagId?.Any() ?? false))
                return new("Nenhuma tag informada");

            var category = Repository.Collection.FindOne(Query.And(Query<WixAllyTag>.EQ(x => x.TagId, input.TagId), Query<WixAllyTag>.EQ(x => x.AllyId, input.AllyId)));
            if (category == null)
                Insert(new WixAllyTag(input.AllyId, input.TagId));

            return new(true);
        }

        public DAOActionResultOutput Insert(WixAllyTag obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(WixAllyTag obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(WixAllyTag obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(WixAllyTag obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveByTagId(string tagId)
        {
            if (string.IsNullOrEmpty(tagId))
                return new("Id da Tag não informada");

            var tag = Repository.Collection.FindOne(Query<WixAllyTag>.EQ(x => x.TagId, tagId));
            if (tag == null)
                return new("Tag não encontrada!");

            Remove(tag);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public WixAllyTag FindOne() => Repository.FindOne();

        public WixAllyTag FindOne(Expression<Func<WixAllyTag, bool>> predicate) => Repository.FindOne(predicate);

        public WixAllyTag FindById(string id) => Repository.FindById(id);

        public IEnumerable<WixAllyTag> Find(Expression<Func<WixAllyTag, bool>> predicate) => Repository.Collection.Find(Query<WixAllyTag>.Where(predicate));

        public IEnumerable<WixAllyTag> FindAll() => Repository.FindAll();

        public IEnumerable<string> GetAllyTagsId(string allyId) => Repository.Collection.Find(Query<WixAllyTag>.EQ(x => x.AllyId, allyId)).SetFields(Fields<WixAllyTag>.Include(x => x.TagId))?.Select(x => x.TagId);
    }
}
