using DAO.Base;
using DAO.DBConnection;
using DAO.Hub.AllyDAO;
using DTO.General.Base.Api.Output;
using DTO.General.DAO.Output;
using DTO.Hub.Application.Youtube.Database;
using DTO.Hub.Application.Youtube.Input;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAO.Hub.Application.Youtube
{
    public class YoutubeAllyPlaylistDAO : IBaseDAO<YoutubeAllyPlaylist>
    {
        protected HubAllyDAO HubAllyDAO;
        internal RepositoryMongo<YoutubeAllyPlaylist> Repository;
        public YoutubeAllyPlaylistDAO(IXDataDatabaseSettings settings)
        {
            HubAllyDAO = new(settings);
            Repository = new(settings?.MongoDBSettings);
        }

        public DAOActionResultOutput InsertAllyPlaylist(HubYoutubeAllyPlaylistInput input)
        {
            if (string.IsNullOrEmpty(input?.AllyId))
                return new("Aliado não informado");

            if (!(input.PlaylistId?.Any() ?? false))
                return new("Nenhuma playlist informada");

            var playlist = Repository.Collection.FindOne(Query.And(Query<YoutubeAllyPlaylist>.EQ(x => x.PlaylistId, input.PlaylistId), Query<YoutubeAllyPlaylist>.EQ(x => x.AllyId, input.AllyId)));
            if (playlist == null)
                Insert(new YoutubeAllyPlaylist(input.AllyId, input.PlaylistId, input.Highlighted));

            return new(true);
        }

        public DAOActionResultOutput Insert(YoutubeAllyPlaylist obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(YoutubeAllyPlaylist obj)
        {
            if (obj.Highlighted)
            {
                var existingHighlited = FindOne(x => x.AllyId == obj.AllyId && x.Highlighted == true);
                if (!string.IsNullOrEmpty(existingHighlited?.Id) && existingHighlited.Id != obj.Id)
                    return new("Já existe uma Playlist marcada como destaque!");
            }

            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(YoutubeAllyPlaylist obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(YoutubeAllyPlaylist obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveByPlaylistId(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new("Id da Playlist não informado");

            var playlists = Repository.Collection.FindOne(Query<YoutubeAllyPlaylist>.EQ(x => x.PlaylistId, id));
            if (playlists == null)
                return new("Playlist não encontrada");

            return Remove(playlists);
        }

        public DAOActionResultOutput RemovePlaylists(string playlistId)
        {
            if (string.IsNullOrEmpty(playlistId))
                return new("Id da Playlist não informado");

            var playlists = Repository.Collection.Find(Query<YoutubeAllyPlaylist>.EQ(x => x.PlaylistId, playlistId));
            if (playlists == null)
                return new("Playlist não encontrada");

            Repository.Collection.Remove(Query<YoutubeAllyPlaylist>.EQ(x => x.PlaylistId, playlistId));

            return new(true);
        }

        public DAOActionResultOutput RemovePlaylistsOnUpdate(string playlistId, string allyId)
        {
            if (string.IsNullOrEmpty(playlistId))
                return new("Id da Playlist não informado");

            if (string.IsNullOrEmpty(allyId))
                return new("Id da aliado não informado");

            var playlists = Repository.Collection.Find(Query.And(Query<YoutubeAllyPlaylist>.EQ(x => x.PlaylistId, playlistId), Query<YoutubeAllyPlaylist>.NE(x => x.AllyId, allyId)));
            if (playlists == null)
                return new("Playlist não encontrada");

            Repository.Collection.Remove(Query.And(
                Query<YoutubeAllyPlaylist>.EQ(x => x.PlaylistId, playlistId),
                Query<YoutubeAllyPlaylist>.NE(x => x.AllyId, allyId)));

            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public YoutubeAllyPlaylist FindOne() => Repository.FindOne();

        public YoutubeAllyPlaylist FindOne(Expression<Func<YoutubeAllyPlaylist, bool>> predicate) => Repository.FindOne(predicate);

        public YoutubeAllyPlaylist FindById(string id) => Repository.FindById(id);

        public IEnumerable<YoutubeAllyPlaylist> Find(Expression<Func<YoutubeAllyPlaylist, bool>> predicate) => Repository.Collection.Find(Query<YoutubeAllyPlaylist>.Where(predicate));

        public IEnumerable<YoutubeAllyPlaylist> FindAll() => Repository.FindAll();
    }
}
