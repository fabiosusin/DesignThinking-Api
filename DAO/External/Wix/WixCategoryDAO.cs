using DAO.Base;
using DAO.DBConnection;
using DAO.Hub.Application.Wix;
using DTO.External.Wix.Database;
using DTO.External.Wix.Input;
using DTO.External.Wix.Output;
using DTO.General.DAO.Output;
using DTO.Hub.Application.Wix.Database;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace DAO.External.Wix
{
    public class WixCategoryDAO : IBaseDAO<WixCategory>
    {
        protected WixAllyCategoryDAO WixAllyCategoryDAO;
        internal RepositoryMongo<WixCategory> Repository;
        public WixCategoryDAO(IXDataDatabaseSettings settings)
        {
            WixAllyCategoryDAO = new(settings);
            Repository = new(settings?.MongoDBSettings);
        }

        public DAOActionResultOutput Insert(WixCategory obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(WixCategory obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(WixCategory obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(WixCategory obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public WixCategory FindOne() => Repository.FindOne();

        public WixCategory FindOne(Expression<Func<WixCategory, bool>> predicate) => Repository.FindOne(predicate);

        public WixCategory FindById(string id) => Repository.FindById(id);

        public IEnumerable<WixCategory> Find(Expression<Func<WixCategory, bool>> predicate) => Repository.Collection.Find(Query<WixCategory>.Where(predicate));

        public IEnumerable<WixCategory> FindAll() => Repository.FindAll();

        public IEnumerable<string> GetWixCategoriesId(IEnumerable<string> categoriesId) => Repository.Collection.Find(Query<WixCategory>.In(x => x.Id, categoriesId)).SetFields(Fields<WixCategory>.Include(x => x.WixCategoryId))?.Select(x => x.WixCategoryId);

        public List<WixCategoryListData> List(WixCategoryListInput input)
        {
            IEnumerable<WixCategory> categories;
            if (input == null)
                categories = FindAll();
            else if (input.Paginator == null)
                categories = Repository.Collection.Find(GenerateFilters(input.Filters));
            else
                categories = Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage);

            if (!(categories?.Any() ?? false))
                return null;

            var result = new List<WixCategoryListData>();
            var allyCategories = !string.IsNullOrEmpty(input?.Filters?.AllyId) ? WixAllyCategoryDAO.Find(x => x.AllyId == input.Filters.AllyId) : null;
            var onlyLinked = input?.Filters?.OnlyLinked ?? false;

            foreach (var item in categories)
            {
                var category = new WixCategoryListData(item, allyCategories?.FirstOrDefault(x => x.CategoryId == item.Id) != null);
                if (onlyLinked && !category.Linked)
                    continue;

                result.Add(category);
            }

            return result;
        }

        private static IMongoQuery GenerateFilters(WixCategoryFiltersInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var queryList = new List<IMongoQuery>();

            if (!string.IsNullOrEmpty(input.Title))
                queryList.Add(Query<WixCategory>.Matches(x => x.Title, $"(?i).*{string.Join(".*", Regex.Split(input.Title, @"\s+").Select(x => Regex.Escape(x)))}.*"));

            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }
    }
}
