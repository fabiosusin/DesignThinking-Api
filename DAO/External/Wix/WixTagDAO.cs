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
    public class WixTagDAO : IBaseDAO<WixTag>
    {
        protected WixAllyTagDAO WixAllyTagDAO;
        internal RepositoryMongo<WixTag> Repository;
        public WixTagDAO(IXDataDatabaseSettings settings)
        {
            WixAllyTagDAO = new(settings);
            Repository = new(settings?.MongoDBSettings);
        }

        public DAOActionResultOutput Insert(WixTag obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(WixTag obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(WixTag obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(WixTag obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public WixTag FindOne() => Repository.FindOne();

        public WixTag FindOne(Expression<Func<WixTag, bool>> predicate) => Repository.FindOne(predicate);

        public WixTag FindById(string id) => Repository.FindById(id);

        public IEnumerable<string> GetWixTagsId(IEnumerable<string> tagsId) => Repository.Collection.Find(Query<WixTag>.In(x => x.Id, tagsId)).SetFields(Fields<WixTag>.Include(x => x.WixTagId))?.Select(x => x.WixTagId);

        public IEnumerable<WixTag> Find(Expression<Func<WixTag, bool>> predicate) => Repository.Collection.Find(Query<WixTag>.Where(predicate));

        public IEnumerable<WixTag> FindAll() => Repository.FindAll();

        public List<WixTagListData> List(WixTagListInput input)
        {
            IEnumerable<WixTag> tags;
            if (input == null)
                tags = FindAll();
            else if (input.Paginator == null)
                tags = Repository.Collection.Find(GenerateFilters(input.Filters));
            else
                tags = Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage);

            if (!(tags?.Any() ?? false))
                return null;

            var result = new List<WixTagListData>();
            var allyCategories = !string.IsNullOrEmpty(input?.Filters?.AllyId) ? WixAllyTagDAO.Find((WixAllyTag x) => x.AllyId == input.Filters.AllyId) : null;
            var onlyLinked = input?.Filters?.OnlyLinked ?? false;

            foreach (var item in tags)
            {
                var category = new WixTagListData(item, allyCategories?.FirstOrDefault(x => x.TagId == item.Id) != null);
                if (onlyLinked && !category.Linked)
                    continue;

                result.Add(category);
            }

            return result;
        }

        private static IMongoQuery GenerateFilters(WixTagFiltersInput input)
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
