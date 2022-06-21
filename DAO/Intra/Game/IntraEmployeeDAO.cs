using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Intra.Game.Database;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAO.Intra.Game
{
    public class IntraGameDAO : IBaseDAO<IntraGame>
    {
        internal RepositoryMongo<IntraGame> Repository;
        public IntraGameDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(IntraGame obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(IntraGame obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(IntraGame obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(IntraGame obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public IntraGame FindOne() => Repository.FindOne();

        public IntraGame FindOne(Expression<Func<IntraGame, bool>> predicate) => Repository.FindOne(predicate);

        public IntraGame FindById(string id) => Repository.FindById(id);

        public IEnumerable<IntraGame> Find(Expression<Func<IntraGame, bool>> predicate) => Repository.Collection.Find(Query<IntraGame>.Where(predicate));

        public IEnumerable<IntraGame> FindAll() => Repository.FindAll();
    }
}
