using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Mobile.Surf.Database;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DAO.Mobile.Surf
{
    public class SurfMobilePlanDAO : IBaseDAO<SurfMobilePlan>
    {
        internal RepositoryMongo<SurfMobilePlan> Repository;
        public SurfMobilePlanDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(SurfMobilePlan obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(SurfMobilePlan obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(SurfMobilePlan obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(SurfMobilePlan obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public SurfMobilePlan FindOne() => Repository.FindOne();

        public SurfMobilePlan FindOne(Expression<Func<SurfMobilePlan, bool>> predicate) => Repository.FindOne(predicate);

        public SurfMobilePlan FindById(string id) => Repository.FindById(id);

        public IEnumerable<SurfMobilePlan> Find(Expression<Func<SurfMobilePlan, bool>> predicate) => Repository.Collection.Find(Query<SurfMobilePlan>.Where(predicate));

        public IEnumerable<SurfMobilePlan> FindAll() => Repository.FindAll();
    }
}
