using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Integration.SendPulse.Database;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAO.Integration.SendPulse
{
    public class SendPulseTokenInfoDAO : IBaseDAO<SendPulseTokenInfo>
    {
        internal RepositoryMongo<SendPulseTokenInfo> Repository;
        public SendPulseTokenInfoDAO(IXDataDatabaseSettings settings)
        {
            Repository = new(settings?.MongoDBSettings);
        }

        public DAOActionResultOutput Insert(SendPulseTokenInfo obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(SendPulseTokenInfo obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(SendPulseTokenInfo obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(SendPulseTokenInfo obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public SendPulseTokenInfo FindOne() => Repository.FindOne();

        public SendPulseTokenInfo FindOne(Expression<Func<SendPulseTokenInfo, bool>> predicate) => Repository.FindOne(predicate);

        public SendPulseTokenInfo FindById(string id) => Repository.FindById(id);

        public IEnumerable<SendPulseTokenInfo> Find(Expression<Func<SendPulseTokenInfo, bool>> predicate) => Repository.Collection.Find(Query<SendPulseTokenInfo>.Where(predicate));

        public IEnumerable<SendPulseTokenInfo> FindAll() => Repository.FindAll();
    }
}
