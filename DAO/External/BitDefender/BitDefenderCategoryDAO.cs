using DAO.Base;
using DAO.DBConnection;
using DTO.External.BitDefender.Database;
using DTO.General.DAO.Output;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAO.External.BitDefender
{
    public class BitDefenderCategoryDAO : IBaseDAO<BitDefenderCategory>
    {
        internal RepositoryMongo<BitDefenderCategory> Repository;
        public BitDefenderCategoryDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(BitDefenderCategory obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(BitDefenderCategory obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(BitDefenderCategory obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(BitDefenderCategory obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public BitDefenderCategory FindOne() => Repository.FindOne();

        public BitDefenderCategory FindOne(Expression<Func<BitDefenderCategory, bool>> predicate) => Repository.FindOne(predicate);

        public BitDefenderCategory FindById(string id) => Repository.FindById(id);

        public IEnumerable<BitDefenderCategory> Find(Expression<Func<BitDefenderCategory, bool>> predicate) => Repository.Collection.Find(Query<BitDefenderCategory>.Where(predicate));

        public IEnumerable<BitDefenderCategory> FindAll() => Repository.FindAll();
    }
}
