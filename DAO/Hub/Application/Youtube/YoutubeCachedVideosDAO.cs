using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Hub.Application.Youtube.Database;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace DAO.Hub.Application.Youtube
{
    public class YoutubeCachedVideosDAO
    {
        internal RepositoryMongo<YoutubeCachedVideos> Repository;
        public YoutubeCachedVideosDAO(IXDataDatabaseSettings settings)
        {
            Repository = new(settings?.MongoDBSettings);
        }

        public DAOActionResultOutput Insert(YoutubeCachedVideos obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(YoutubeCachedVideos obj)
        {
            var video = FindById(obj.Id);

            if (video == null)
                return new("Video não encontrado!");

            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(YoutubeCachedVideos obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(YoutubeCachedVideos obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public YoutubeCachedVideos FindOne() => Repository.FindOne();

        public YoutubeCachedVideos FindOne(Expression<Func<YoutubeCachedVideos, bool>> predicate) => Repository.FindOne(predicate);

        public YoutubeCachedVideos FindPlaylistData(string channelId, string playlistId, string prev, string next) => Repository.Collection.FindOne(Query.And(
            Query<YoutubeCachedVideos>.EQ(x => x.YoutubeChannelId, channelId), 
            Query<YoutubeCachedVideos>.EQ(x => x.YoutubePlaylistId, playlistId),
            Query<YoutubeCachedVideos>.EQ(x => x.PrevPageToken, string.IsNullOrEmpty(prev) ? null : prev),
            Query<YoutubeCachedVideos>.EQ(x => x.NextPageToken, string.IsNullOrEmpty(next) ? null : next)));

        public YoutubeCachedVideos FindById(string id) => Repository.FindById(id);

        public IEnumerable<YoutubeCachedVideos> Find(Expression<Func<YoutubeCachedVideos, bool>> predicate) => Repository.Collection.Find(Query<YoutubeCachedVideos>.Where(predicate));

        public IEnumerable<YoutubeCachedVideos> FindAll() => Repository.FindAll();

        public List<YoutubeCachedVideos> FindByText(string search, string channelId) => Repository.Collection.Find(Query.And(
            Query<YoutubeCachedVideos>.EQ(x => x.YoutubeChannelId, channelId),
            Query.Or(
                Query<YoutubeCachedVideos>.ElemMatch(x => x.Data.Items, x => x.Matches(y => y.Snippet.Title, $"(?i).{string.Join(".", Regex.Split(search, @"\s+").Select(x => Regex.Escape(x)))}.*")),
                Query<YoutubeCachedVideos>.ElemMatch(x => x.Data.Items, x => x.Matches(y => y.Snippet.Description, $"(?i).{string.Join(".", Regex.Split(search, @"\s+").Select(x => Regex.Escape(x)))}.*"))
                ))).ToList();
    }
}

