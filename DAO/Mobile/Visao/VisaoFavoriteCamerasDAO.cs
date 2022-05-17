using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Mobile.Visao.Database;
using DTO.Mobile.Visao.Input;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAO.Mobile.Visao
{
    public class VisaoFavoriteCamerasDAO : IBaseDAO<AppVisaoUserFavoriteCamera>
    {
        internal RepositoryMongo<AppVisaoUserFavoriteCamera> Repository;
        public VisaoFavoriteCamerasDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(AppVisaoUserFavoriteCamera obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(AppVisaoUserFavoriteCamera obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(AppVisaoUserFavoriteCamera obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(AppVisaoUserFavoriteCamera obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public AppVisaoUserFavoriteCamera FindOne() => Repository.FindOne();

        public AppVisaoUserFavoriteCamera FindOne(Expression<Func<AppVisaoUserFavoriteCamera, bool>> predicate) => Repository.FindOne(predicate);

        public AppVisaoUserFavoriteCamera FindById(string id) => Repository.FindById(id);

        public IEnumerable<AppVisaoUserFavoriteCamera> Find(Expression<Func<AppVisaoUserFavoriteCamera, bool>> predicate) => Repository.Collection.Find(Query<AppVisaoUserFavoriteCamera>.Where(predicate));

        public IEnumerable<AppVisaoUserFavoriteCamera> FindAll() => Repository.FindAll();
        public IEnumerable<AppVisaoUserFavoriteCamera> Find(AppFavoriteCameraListInput input) => input == null ? 
            FindAll() : input.Paginator == null ?
            Repository.Collection.Find(GenerateFilters(input.Filters)) : Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage);

        private static IMongoQuery GenerateFilters(AppFiltersFavoriteCameraInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            return !string.IsNullOrEmpty(input?.UserId) ? Query.And(Query<AppVisaoUserFavoriteCamera>.EQ(x => x.UserId, input.UserId)) : emptyResult;
        }
    }
}
