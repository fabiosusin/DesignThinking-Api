using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.General.Image.Input;
using DTO.Hub.Application.Sponsor.Database;
using DTO.Hub.Application.Sponsor.Input;
using DTO.Hub.Application.Sponsor.Output;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace DAO.Hub.Application.Database
{
    public class AppSponsorDAO : IBaseDAO<AppSponsor>
    {
        internal RepositoryMongo<AppSponsor> Repository;
        public AppSponsorDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(AppSponsor obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(AppSponsor obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(AppSponsor obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(AppSponsor obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public AppSponsor FindOne() => Repository.FindOne();

        public AppSponsor FindOne(Expression<Func<AppSponsor, bool>> predicate) => Repository.FindOne(predicate);

        public AppSponsor FindById(string id) => Repository.FindById(id);

        public IEnumerable<AppSponsor> Find(Expression<Func<AppSponsor, bool>> predicate) => Repository.Collection.Find(Query<AppSponsor>.Where(predicate));

        public IEnumerable<AppSponsor> FindAll() => Repository.FindAll();

        public List<AppSponsorListData> List(HubAppSponsorListInput input)
        {
            IEnumerable<AppSponsor> categories;
            if (input == null)
                categories = FindAll();
            else if (input.Paginator == null)
                categories = Repository.Collection.Find(GenerateFilters(input.Filters));
            else
                categories = Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage);

            if (!(categories?.Any() ?? false))
                return null;

            var result = new List<AppSponsorListData>();
            var imgSize = input?.Filters?.ImageSize ?? ListResolutionsSize.Url1024;
            
            foreach (var item in categories)
                result.Add(new(item, item.Image?.GetImage(imgSize, FileType.Png)));

            return result;
        }

        private static IMongoQuery GenerateFilters(HubAppSponsorFiltersInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var queryList = new List<IMongoQuery>();

            if (!string.IsNullOrEmpty(input.Title))
                queryList.Add(Query<AppSponsor>.Matches(x => x.Title, $"(?i).*{string.Join(".*", Regex.Split(input.Title, @"\s+").Select(x => Regex.Escape(x)))}.*"));

            if (!string.IsNullOrEmpty(input.AllyId))
                queryList.Add(Query<AppSponsor>.EQ(x => x.AllyId, input.AllyId));

            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }
    }
}
