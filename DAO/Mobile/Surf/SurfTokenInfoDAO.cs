using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Integration.Surf.Token.Database;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAO.Mobile.Surf
{
    public class SurfTokenInfoDAO : IBaseDAO<SurfTokenInfo>
    {
        internal RepositoryMongo<SurfTokenInfo> Repository;
        public SurfTokenInfoDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(SurfTokenInfo obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(SurfTokenInfo obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(SurfTokenInfo data)
        {
            var existing = FindOne();
            if (!string.IsNullOrEmpty(existing?.Id) && existing.Id == data.Id)
            {
                existing.Token = data.Token;
                existing.RefreshToken = data.RefreshToken;
                existing.Expiration = data.Expiration;
                existing.Created = data.Created;

                return Update(existing);
            }

            return Insert(data);
        }

        public DAOActionResultOutput Remove(SurfTokenInfo obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public SurfTokenInfo FindOne() => Repository.FindOne();

        public SurfTokenInfo FindOne(Expression<Func<SurfTokenInfo, bool>> predicate) => Repository.FindOne(predicate);

        public SurfTokenInfo FindById(string id) => Repository.FindById(id);

        public IEnumerable<SurfTokenInfo> Find(Expression<Func<SurfTokenInfo, bool>> predicate) => Repository.Collection.Find(Query<SurfTokenInfo>.Where(predicate));

        public IEnumerable<SurfTokenInfo> FindAll() => Repository.FindAll();
    }
}
