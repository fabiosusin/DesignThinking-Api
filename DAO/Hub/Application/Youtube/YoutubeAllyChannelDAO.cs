using DAO.Base;
using DAO.DBConnection;
using DAO.Hub.AllyDAO;
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
    public class YoutubeAllyChannelDAO : IBaseDAO<YoutubeAllyChannel>
    {
        protected HubAllyDAO HubAllyDAO;
        internal RepositoryMongo<YoutubeAllyChannel> Repository;
        public YoutubeAllyChannelDAO(IXDataDatabaseSettings settings)
        {
            HubAllyDAO = new(settings);
            Repository = new(settings?.MongoDBSettings);
        }

        public DAOActionResultOutput InsertAllyChannel(HubYoutubeAllyChannelInput input)
        {
            if (string.IsNullOrEmpty(input?.AllyId))
                return new("Aliado não informado");

            if (!(input.ChannelId?.Any() ?? false))
                return new("Nenhuma playlist informada");

            var playlist = Repository.Collection.FindOne(Query.And(Query<YoutubeAllyChannel>.EQ(x => x.ChannelId, input.ChannelId), Query<YoutubeAllyChannel>.EQ(x => x.AllyId, input.AllyId)));
            if (playlist == null)
                Insert(new YoutubeAllyChannel(input.AllyId, input.ChannelId));

            return new(true);
        }

        public DAOActionResultOutput Insert(YoutubeAllyChannel obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(YoutubeAllyChannel obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(YoutubeAllyChannel obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(YoutubeAllyChannel obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveAllyChannel(string id, string allyId)
        {
            if (string.IsNullOrEmpty(id))
                return new("Id do canal não informado");

            Repository.Collection.Remove(Query.And(Query<YoutubeAllyChannel>.EQ(x => x.ChannelId, id), Query<YoutubeAllyChannel>.EQ(x => x.AllyId, allyId)));

            return new(true);
        }

        public DAOActionResultOutput RemoveChannels(string channelId)
        {
            if (string.IsNullOrEmpty(channelId))
                return new("Id do canal não informado");

            var channel = Repository.Collection.Find(Query<YoutubeAllyChannel>.EQ(x => x.ChannelId, channelId));
            if (channel == null)
                return new("Canal não encontrado");

            Repository.Collection.Remove(Query<YoutubeAllyChannel>.EQ(x => x.ChannelId, channelId));

            return new(true);
        }

        public DAOActionResultOutput RemoveChannelsOnUpdate(string channelId, string allyId)
        {
            if (string.IsNullOrEmpty(channelId))
                return new("Id do canal não informado");

            if (string.IsNullOrEmpty(allyId))
                return new("Id do aliado não informado");

            var channel = Repository.Collection.Find(Query.And(Query<YoutubeAllyChannel>.EQ(x => x.ChannelId, channelId), Query<YoutubeAllyChannel>.NE(x => x.AllyId, allyId)));
            if (channel == null)
                return new("Canal não encontrado");

            Repository.Collection.Remove(Query.And(
                Query<YoutubeAllyChannel>.EQ(x => x.ChannelId, channelId),
                Query<YoutubeAllyChannel>.NE(x => x.AllyId, allyId)));

            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public YoutubeAllyChannel FindOne() => Repository.FindOne();

        public YoutubeAllyChannel FindOne(Expression<Func<YoutubeAllyChannel, bool>> predicate) => Repository.FindOne(predicate);

        public YoutubeAllyChannel FindById(string id) => Repository.FindById(id);

        public IEnumerable<YoutubeAllyChannel> Find(Expression<Func<YoutubeAllyChannel, bool>> predicate) => Repository.Collection.Find(Query<YoutubeAllyChannel>.Where(predicate));

        public IEnumerable<YoutubeAllyChannel> FindAll() => Repository.FindAll();
    }
}
