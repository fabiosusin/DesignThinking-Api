using DAO.Base;
using DAO.DBConnection;
using DTO.External.Visao.Database;
using DTO.External.Visao.Input;
using DTO.General.DAO.Output;
using DTO.Mobile.Visao.Input;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAO.External.Visao
{
    public class VisaoCameraDAO : IBaseDAO<VisaoCamera>
    {
        internal RepositoryMongo<VisaoCamera> Repository;
        public VisaoCameraDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(VisaoCamera obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(VisaoCamera obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(VisaoCamera obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(VisaoCamera obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public VisaoCamera FindOne() => Repository.FindOne();

        public VisaoCamera FindOne(Expression<Func<VisaoCamera, bool>> predicate) => Repository.FindOne(predicate);

        public VisaoCamera FindById(string id) => Repository.FindById(id);

        public IEnumerable<VisaoCamera> Find(Expression<Func<VisaoCamera, bool>> predicate) => Repository.Collection.Find(Query<VisaoCamera>.Where(predicate));

        public IEnumerable<VisaoCamera> FindAll() => Repository.FindAll();

        public IEnumerable<VisaoCamera> Find(CameraListExternalInput input) => input == null ? FindAll() : input.Paginator == null ? Repository.Collection.Find(GenerateFilters(input?.Filters)) : Repository.Collection.Find(GenerateFilters(input?.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage);

        public IEnumerable<VisaoCamera> Find(AppFiltersCameraInput input) => input == null ? FindAll() : Repository.Collection.Find(GenerateFilters(input));

        public IEnumerable<string> GetCities(AppFiltersCameraInput input) => Repository.Collection.Distinct("Address.City", GenerateFilters(input)).Select(x => x.ToString());

        private static IMongoQuery GenerateFilters(FiltersCameraExternalInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var queryList = new List<IMongoQuery>();

            if (!string.IsNullOrEmpty(input.CameraId))
                queryList.Add(Query<VisaoCamera>.EQ(x => x.Id, input.CameraId));

            if (input.OthersWithFreeAccess)
            {
                if (!string.IsNullOrEmpty(input.AllyId))
                    queryList.Add(Query.Or(Query<VisaoCamera>.EQ(x => x.AllyId, input.AllyId), Query<VisaoCamera>.EQ(x => x.FreeAccess, true)));
                else
                    queryList.Add(Query<VisaoCamera>.EQ(x => x.FreeAccess, true));
            }
            else if (!string.IsNullOrEmpty(input.AllyId))
                queryList.Add(Query<VisaoCamera>.EQ(x => x.AllyId, input.AllyId));

            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }

        private static IMongoQuery GenerateFilters(AppFiltersCameraInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var queryList = new List<IMongoQuery>();

            if (!string.IsNullOrEmpty(input.CameraName))
                queryList.Add(Query<VisaoCamera>.EQ(x => x.Name, input.CameraName));

            if (!string.IsNullOrEmpty(input.State))
                queryList.Add(Query<VisaoCamera>.EQ(x => x.Address.State, input.State));

            if (input.OthersWithFreeAccess)
            {
                if (!string.IsNullOrEmpty(input.AllyId))
                    queryList.Add(Query.Or(Query<VisaoCamera>.EQ(x => x.AllyId, input.AllyId), Query<VisaoCamera>.EQ(x => x.FreeAccess, true)));
                else
                    queryList.Add(Query<VisaoCamera>.EQ(x => x.FreeAccess, true));
            }
            else if (!string.IsNullOrEmpty(input.AllyId))
                queryList.Add(Query<VisaoCamera>.EQ(x => x.AllyId, input.AllyId));

            if (input.Cities?.Any() ?? false)
                queryList.Add(Query<VisaoCamera>.In(x => x.Address.City, input.Cities));

            if (!string.IsNullOrEmpty(input.CameraId))
                queryList.Add(Query<VisaoCamera>.EQ(x => x.Id, input.CameraId));

            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }
    }
}
