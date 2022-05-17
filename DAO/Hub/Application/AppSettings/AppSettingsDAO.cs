using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Hub.Application.AppSettings.Database;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAO.Hub.Application.Settings
{
    public class AppSettingsDAO : IBaseDAO<AppSettings>
    {
        internal RepositoryMongo<AppSettings> Repository;
        public AppSettingsDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(AppSettings obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(AppSettings obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(AppSettings obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(AppSettings obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public AppSettings FindOne() => Repository.FindOne();

        public AppSettings FindOne(Expression<Func<AppSettings, bool>> predicate) => Repository.FindOne(predicate);

        public AppSettings FindById(string id) => Repository.FindById(id);

        public IEnumerable<AppSettings> Find(Expression<Func<AppSettings, bool>> predicate) => Repository.Collection.Find(Query<AppSettings>.Where(predicate));

        public IEnumerable<AppSettings> FindAll() => Repository.FindAll();
    }
}
