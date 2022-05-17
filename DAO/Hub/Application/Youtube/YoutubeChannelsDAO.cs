using DAO.Base;
using DAO.DBConnection;
using DAO.Hub.Application.Youtube;
using DTO.General.DAO.Output;
using DTO.Hub.Application.Youtube.Input;
using DTO.Hub.Application.Youtube.Output;
using DTO.Integration.Youtube.Database;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAO.Integration.Youtube
{
    public class YoutubeChannelsDAO : IBaseDAO<YoutubeChannel>
    {
        internal RepositoryMongo<YoutubeChannel> Repository;
        private readonly YoutubeAllyChannelDAO YoutubeAllyChannelDAO;
        private readonly YoutubePlaylistDAO YoutubePlaylistDAO;
        private readonly YoutubeAllyPlaylistDAO YoutubeAllyPlaylistDAO;
        public YoutubeChannelsDAO(IXDataDatabaseSettings settings)
        {
            Repository = new(settings?.MongoDBSettings);
            YoutubeAllyChannelDAO = new(settings);
            YoutubePlaylistDAO = new(settings);
            YoutubeAllyPlaylistDAO = new(settings);
        }

        public DAOActionResultOutput Insert(YoutubeChannel obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(YoutubeChannel obj)
        {
            var channel = FindById(obj.Id);

            if (channel == null)
                return new("Canal não encontrado!");

            var playlistsIdsOnChannel = YoutubePlaylistDAO.Find(x => x.ChannelId == channel.Id);

            foreach (var playlist in playlistsIdsOnChannel)
            {
                playlist.YoutubeChannelId = obj.YoutubeChannelId;
                YoutubePlaylistDAO.Update(playlist);
            }

            if (channel.IsGlobal && !obj.IsGlobal)
            {
                foreach (var Id in playlistsIdsOnChannel.Select(x => x.Id))
                {
                    YoutubeAllyPlaylistDAO.RemovePlaylistsOnUpdate(Id, channel.AllyId);
                    YoutubePlaylistDAO.RemovePlaylistOnUpdate(Id, channel.AllyId);
                }

                YoutubeAllyChannelDAO.RemoveChannelsOnUpdate(channel.Id, channel.AllyId);
            }

            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(YoutubeChannel obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(YoutubeChannel obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public DAOActionResultOutput RemoveChannel(string channelId)
        {
            if (string.IsNullOrEmpty(channelId))
                return new("Id do canal não informado");

            Repository.Collection.Remove(Query<YoutubeChannel>.EQ(x => x.Id, channelId));

            YoutubeAllyChannelDAO.RemoveChannels(channelId);

            return new(true);
        }

        public YoutubeChannel FindOne() => Repository.FindOne();

        public YoutubeChannel FindOne(Expression<Func<YoutubeChannel, bool>> predicate) => Repository.FindOne(predicate);

        public YoutubeChannel FindById(string id) => Repository.FindById(id);

        public IEnumerable<YoutubeChannel> Find(Expression<Func<YoutubeChannel, bool>> predicate) => Repository.Collection.Find(Query<YoutubeChannel>.Where(predicate));

        public IEnumerable<YoutubeChannel> FindAll() => Repository.FindAll();

        public List<YoutubeChannelsListData> List(HubYoutubeListInput input)
        {
            IEnumerable<YoutubeChannel> channels;
            if (input == null)
                channels = FindAll();
            else if (input.Paginator == null)
                channels = Repository.Collection.Find(GenerateFilters(input.Filters));
            else
                channels = Repository.Collection.Find(GenerateFilters(input.Filters)).SetSkip((input.Paginator.Page > 0 ? input.Paginator.Page - 1 : 0) * input.Paginator.ResultsPerPage).SetLimit(input.Paginator.ResultsPerPage);

            if (!(channels?.Any() ?? false))
                return null;

            var allyChannels = !string.IsNullOrEmpty(input?.Filters?.AllyId) ? YoutubeAllyChannelDAO.Find(x => x.AllyId == input.Filters.AllyId) : null;
            var result = new List<YoutubeChannelsListData>();
            
            foreach (var item in channels)
            {
                var allyChannel = allyChannels?.FirstOrDefault(x => x.ChannelId == item.Id);
                var channel = new YoutubeChannelsListData(item, allyChannel != null);

                result.Add(channel);
            }

            return result;
        }

        private static IMongoQuery GenerateFilters(HubYoutubeFiltersInput input)
        {
            var emptyResult = Query.And(Query.Empty);
            if (input == null)
                return emptyResult;

            var queryList = new List<IMongoQuery>();

            if (!string.IsNullOrEmpty(input.AllyId))
                queryList.Add(Query.Or(
                    Query<YoutubeChannelsListData>.EQ(x => x.AllyId, input.AllyId),
                    Query<YoutubeChannelsListData>.EQ(x => x.IsGlobal, true)));
            else
                queryList.Add(Query<YoutubeChannelsListData>.EQ(x => x.IsGlobal, true));

            return queryList.Any() ? Query.And(queryList) : emptyResult;
        }
    }
}
