using DAO.Base;
using DAO.DBConnection;
using DAO.Hub.Application.Wix;
using DTO.External.Wix.Database;
using DTO.General.DAO.Output;
using DTO.Mobile.News.Input;
using DTO.Mobile.News.Output;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace DAO.External.Wix
{
    public class WixPostDAO : IBaseDAO<WixPost>
    {
        protected WixCategoryDAO WixCategoryDAO;
        protected WixTagDAO WixTagDAO;
        protected WixAllyCategoryDAO WixAllyCategoryDAO;
        internal RepositoryMongo<WixPost> Repository;
        public WixPostDAO(IXDataDatabaseSettings settings)
        {
            WixAllyCategoryDAO = new(settings);
            WixCategoryDAO = new(settings);
            WixTagDAO = new(settings);
            Repository = new(settings?.MongoDBSettings);
        }

        public DAOActionResultOutput Insert(WixPost obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(WixPost obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(WixPost obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(WixPost obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public WixPost FindOne() => Repository.FindOne();

        public WixPost FindOne(Expression<Func<WixPost, bool>> predicate) => Repository.FindOne(predicate);

        public WixPost FindById(string id) => Repository.FindById(id);

        public IEnumerable<WixPost> Find(Expression<Func<WixPost, bool>> predicate) => Repository.Collection.Find(Query<WixPost>.Where(predicate));

        public IEnumerable<WixPost> FindAll() => Repository.FindAll();

        public AppNewsOutput FindNewsOutputById(string id)
        {
            var post = Repository.FindById(id);
            return post == null ? null : new AppNewsOutput(post)
            {
                Categories = WixCategoryDAO.Find(x => post.WixCategoryIds.Contains(x.WixCategoryId))?.Select(x => new MobileNewsCategory(x.Id, x.Title)),
                Tags = WixTagDAO.Find(x => post.WixTagIds.Contains(x.WixTagId))?.Select(x => new MobileNewsCategory(x.Id, x.Title))
            };
        }

        public List<AppNewsOutput> List(AppNewsListInput input)
        {
            IEnumerable<WixPost> posts;
            if (input == null)
                posts = FindAll().OrderByDescending(x => x.PublishedDate);
            if (input.Paginator == null)
                posts = Repository.Collection.Find(GenerateFilters(input.Filters)).SetSortOrder(SortBy<WixPost>.Descending(x => x.PublishedDate)).ToList();
            else
                posts = Repository.Collection.Find(GenerateFilters(input.Filters)).SetSortOrder(SortBy<WixPost>.Descending(x => x.PublishedDate)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage).ToList();

            var result = new List<AppNewsOutput>();
            var categories = WixCategoryDAO.FindAll();
            var tags = WixTagDAO.FindAll();

            foreach (var post in posts)
            {
                var data = new AppNewsOutput(post);

                if (post.WixCategoryIds.Any())
                    data.Categories = categories?.Where(x => post.WixCategoryIds.Contains(x.WixCategoryId))?.Select(x => new MobileNewsCategory(x.Id, x.Title));

                if (post.WixTagIds.Any())
                    data.Tags = tags?.Where(x => post.WixTagIds.Contains(x.WixTagId))?.Select(x => new MobileNewsCategory(x.Id, x.Title));

                result.Add(data);
            }

            return result;
        }

        private IMongoQuery GenerateFilters(AppNewsFiltersInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var queryList = new List<IMongoQuery>();

            if (!string.IsNullOrEmpty(input.Title))
                queryList.Add(Query.Or(
                    Query<WixPost>.Matches(x => x.Title, $"(?i).*{string.Join(".*", Regex.Split(input.Title, @"\s+").Select(x => Regex.Escape(x)))}.*"),
                    Query<WixPost>.Matches(x => x.Content, $"(?i).*{string.Join(".*", Regex.Split(input.Title, @"\s+").Select(x => Regex.Escape(x)))}.*")));

            if (input.Ids?.Any() ?? false)
                queryList.Add(Query<WixPost>.In(x => x.Id, input.Ids));

            if (!string.IsNullOrEmpty(input.WixCategoryId))
                queryList.Add(Query<WixPost>.In(x => x.WixCategoryIds, new List<string> { input.WixCategoryId }));

            if (!string.IsNullOrEmpty(input.WixTagId))
                queryList.Add(Query<WixPost>.In(x => x.WixTagIds, new List<string> { input.WixTagId }));

            if (input.OnlyLinked)
                queryList.Add(Query<WixPost>.In(x => x.WixCategoryIds, WixCategoryDAO.GetWixCategoriesId(WixAllyCategoryDAO.GetAllyCategoriesId(input?.AllyId))));

            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }

    }
}
