using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.General.Image.Input;
using DTO.Hub.Application.Youtube.Database;
using DTO.Hub.Application.Youtube.Input;
using DTO.Hub.Application.Youtube.Output;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace DAO.Hub.Application.Youtube
{
    public class YoutubePlaylistDAO : IBaseDAO<YoutubePlaylist>
    {
        protected YoutubeAllyPlaylistDAO YoutubeAllyPlaylistDAO;
        internal RepositoryMongo<YoutubePlaylist> Repository;
        public YoutubePlaylistDAO(IXDataDatabaseSettings settings)
        {
            Repository = new(settings?.MongoDBSettings);
            YoutubeAllyPlaylistDAO = new(settings);
        }

        public DAOActionResultOutput Insert(YoutubePlaylist obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(YoutubePlaylist obj)
        {
            var playlist = FindById(obj.Id);

            if (playlist == null)
                return new("Playlist não encontrada!");

            if (playlist.IsGlobal && !obj.IsGlobal)
                YoutubeAllyPlaylistDAO.RemovePlaylistsOnUpdate(playlist.Id, playlist.AllyId);

            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(YoutubePlaylist obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(YoutubePlaylist obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public DAOActionResultOutput RemovePlaylist(string playlistId)
        {
            if (string.IsNullOrEmpty(playlistId))
                return new("Id da playlist não informado");

            Repository.Collection.Remove(Query<YoutubePlaylist>.EQ(x => x.Id, playlistId));
            YoutubeAllyPlaylistDAO.RemovePlaylists(playlistId);

            return new(true);
        }

        public DAOActionResultOutput RemovePlaylistOnUpdate(string playlistId, string allyId)
        {
            if (string.IsNullOrEmpty(playlistId))
                return new("Id da playlist não informado");

            if (string.IsNullOrEmpty(playlistId))
                return new("Id do aliado não informado");

            Repository.Collection.Remove(Query.And(
                Query<YoutubePlaylist>.EQ(x => x.Id, playlistId),
                Query<YoutubePlaylist>.NE(x => x.AllyId, allyId)));

            return new(true);
        }

        public YoutubePlaylist FindOne() => Repository.FindOne();

        public YoutubePlaylist FindOne(Expression<Func<YoutubePlaylist, bool>> predicate) => Repository.FindOne(predicate);

        public YoutubePlaylist FindById(string id) => Repository.FindById(id);

        public IEnumerable<YoutubePlaylist> Find(Expression<Func<YoutubePlaylist, bool>> predicate) => Repository.Collection.Find(Query<YoutubePlaylist>.Where(predicate));

        public IEnumerable<YoutubePlaylist> FindAll() => Repository.FindAll();

        public IEnumerable<string> FindAllAllyPlaylistsIds(string allyId) => Repository.Collection.Find(Query<YoutubePlaylist>.Where(x => x.AllyId == allyId)).SetFields(Fields<YoutubePlaylist>.Include(x => x.YoutubePlaylistId)).Select(x => x.YoutubePlaylistId);

        public IEnumerable<YoutubePlaylist> GetAllyPlaylists(string allyId)
        {
            var playlistsAlly = YoutubeAllyPlaylistDAO.Find(x => x.AllyId == allyId);
            if (!(playlistsAlly?.Any() ?? false))
                return null;

            return Repository.Collection.Find(Query<YoutubePlaylist>.In(x => x.Id, playlistsAlly.Select(x => x.PlaylistId)));
        }

        public List<YoutubePlaylistListData> List(HubYoutubeListInput input)
        {
            IEnumerable<YoutubePlaylist> playlists;
            if (input == null)
                playlists = FindAll();
            else if (input.Paginator == null)
                playlists = Repository.Collection.Find(GenerateFilters(input.Filters));
            else
                playlists = Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage);

            if (!(playlists?.Any() ?? false))
                return null;

            var result = new List<YoutubePlaylistListData>();
            var imgSize = input?.Filters?.ImageSize ?? ListResolutionsSize.Url1024;
            var allyPlaylists = !string.IsNullOrEmpty(input?.Filters?.AllyId) ? YoutubeAllyPlaylistDAO.Find(x => x.AllyId == input.Filters.AllyId) : null;
            var onlyLinked = input?.Filters?.OnlyLinked ?? false;

            foreach (var item in playlists)
            {
                var allyPlaylist = allyPlaylists?.FirstOrDefault(x => x.PlaylistId == item.Id);
                var playlist = new YoutubePlaylistListData(item, item.Image?.GetImage(imgSize, FileType.Png), allyPlaylist != null, allyPlaylist?.Highlighted);
                if (onlyLinked && !playlist.Linked)
                    continue;

                result.Add(playlist);
            }

            return result;
        }

        private static IMongoQuery GenerateFilters(HubYoutubeFiltersInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var queryList = new List<IMongoQuery>();
            if (!string.IsNullOrEmpty(input.Name))
                queryList.Add(Query<YoutubePlaylist>.Matches(x => x.YoutubePlaylistName, $"(?i).*{string.Join(".*", Regex.Split(input.Name, @"\s+").Select(x => Regex.Escape(x)))}.*"));

            if (!string.IsNullOrEmpty(input.AllyId))
                queryList.Add(Query.Or(
                    Query<YoutubePlaylist>.EQ(x => x.AllyId, input.AllyId),
                    Query<YoutubePlaylist>.EQ(x => x.IsGlobal, true)));
            else
                queryList.Add(Query<YoutubePlaylist>.EQ(x => x.IsGlobal, true));

            if (input.Tags?.Any() ?? false)
                queryList.Add(Query<YoutubePlaylist>.In(x => x.Tags, input.Tags));

            if (input.ChannelsIds != null)
                queryList.Add(Query<YoutubePlaylist>.In(x => x.YoutubeChannelId, input.ChannelsIds));

            if (!string.IsNullOrEmpty(input.PlaylistId))
                queryList.Add(Query<YoutubePlaylist>.EQ(x => x.YoutubePlaylistId, input.PlaylistId));

            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }
    }
}
