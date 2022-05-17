using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Integration.Youtube.Database;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAO.Integration.Youtube
{
    public class YoutubeApiKeyDAO : IBaseDAO<YoutubeApiKey>
    {
        internal RepositoryMongo<YoutubeApiKey> Repository;
        public YoutubeApiKeyDAO(IXDataDatabaseSettings settings)
        {
            Repository = new(settings?.MongoDBSettings);
        }

        public DAOActionResultOutput Insert(YoutubeApiKey obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(YoutubeApiKey obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(YoutubeApiKey obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(YoutubeApiKey obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public YoutubeApiKey FindOne() => Repository.FindOne();

        public YoutubeApiKey FindOne(Expression<Func<YoutubeApiKey, bool>> predicate) => Repository.FindOne(predicate);

        public YoutubeApiKey FindById(string id) => Repository.FindById(id);

        public IEnumerable<YoutubeApiKey> Find(Expression<Func<YoutubeApiKey, bool>> predicate) => Repository.Collection.Find(Query<YoutubeApiKey>.Where(predicate));

        public IEnumerable<YoutubeApiKey> FindAll() => Repository.FindAll();

        public YoutubeApiKey GetValidKey() => FindOne(x => x.ExpiredIn < DateTime.Now.AddDays(-1));

        public DAOActionResultOutput MarkAsExpired(YoutubeApiKey obj)
        {
            obj.ExpiredIn = DateTime.Now;
            Update(obj);
            return new(true);
        }
    }
}
