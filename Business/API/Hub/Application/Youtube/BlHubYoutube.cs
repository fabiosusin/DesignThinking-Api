using DAO.DBConnection;
using DAO.General.Log;
using DAO.Hub.Application.Youtube;
using DAO.Hub.UserDAO;
using DAO.Integration.Youtube;
using DTO.General.Base.Api.Output;
using DTO.General.Log.Database;
using DTO.General.Log.Enum;
using DTO.Hub.Application.Youtube.Input;
using Newtonsoft.Json;
using System;
using System.Linq;
using static Utils.Extensions.Files.Images.ImageFactory;

namespace Business.API.Hub.Application.Youtube
{
    public class BlHubYoutube
    {
        private readonly YoutubePlaylistDAO YoutubePlaylistDAO;
        private readonly YoutubeAllyPlaylistDAO YoutubeAllyPlaylistDAO;
        private readonly YoutubeChannelsDAO YoutubeChannelsDAO;
        private readonly YoutubeAllyChannelDAO YoutubeAllyChannelDAO;
        private readonly LogHistoryDAO LogHistoryDAO;
        private readonly HubUserDAO HubUserDAO;

        public BlHubYoutube(XDataDatabaseSettings settings)
        {
            YoutubePlaylistDAO = new(settings);
            YoutubeAllyPlaylistDAO = new(settings);
            YoutubeChannelsDAO = new(settings);
            YoutubeAllyChannelDAO = new(settings);
            LogHistoryDAO = new(settings);
            HubUserDAO = new(settings);
        }

        public BaseApiOutput UpsertYoutubePlaylist(HubYoutubePlaylistInput input)
        {
            if (string.IsNullOrEmpty(input?.YoutubePlaylistId))
                return new("Id da playlist não informada!");

            if (string.IsNullOrEmpty(input.YoutubePlaylistName))
                return new("Nome da playlist não informada!");

            if (string.IsNullOrEmpty(input?.YoutubeChannelId))
                return new("Id do canal não informado!");

            if (string.IsNullOrEmpty(input.ImgBase64) && string.IsNullOrEmpty(input.ImgUrl))
                return new("Imagem da playlist não informada!");

            if (YoutubeChannelsDAO.FindOne(x => x.YoutubeChannelId == input.YoutubeChannelId) == null)
                return new("Não existe um canal para esta playlist!");

            var existing = YoutubePlaylistDAO.FindOne(x => x.Id == input.PlaylistId && x.AllyId == input.AllyId);

            if (YoutubePlaylistDAO.FindOne(x => x.YoutubePlaylistName == input.YoutubePlaylistName && x.Id != input.PlaylistId && x.AllyId == input.AllyId) != null)
                return new("Playlist já criada com este Nome!");

            var img = string.IsNullOrEmpty(input.ImgBase64) ? existing?.Image : SaveListResolutions(input.ImgBase64);
            if (img == null)
                return new("Não foi possível salvar o Logo");

            for (var i = 0; i < input.Tags?.Count; i++)
            {
                var tag = input.Tags[i];
                if (tag.StartsWith('#'))
                    tag = tag.Remove(0, 1);

                input.Tags[i] = tag;
            }

            return string.IsNullOrEmpty(existing?.Id) ? YoutubePlaylistDAO.Insert(new(input, img)) : YoutubePlaylistDAO.Update(new(input, img, existing.Id));
        }

        public BaseApiOutput UpsertAllyPlaylist(HubYoutubeAllyPlaylistInput input)
        {
            if (string.IsNullOrEmpty(input?.AllyId))
                return new("Aliado não informado");

            if (!(input.PlaylistId?.Any() ?? false))
                return new("Nenhuma playlist informada");

            return input.Linked ? YoutubeAllyPlaylistDAO.InsertAllyPlaylist(input) : YoutubeAllyPlaylistDAO.RemoveByPlaylistId(input.PlaylistId);
        }

        public BaseApiOutput UpsertAllyPlaylistHighlighted(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new("Nenhuma playlist informada");

            var playlist = YoutubeAllyPlaylistDAO.FindOne(x => x.PlaylistId == id);
            if (playlist == null)
                return new("Nenhuma playlist encontrada");

            playlist.Highlighted = !playlist.Highlighted;
            return YoutubeAllyPlaylistDAO.Update(playlist);
        }

        public BaseApiOutput UpsertYoutubeChannel(HubYoutubeChannelInput input)
        {
            if (string.IsNullOrEmpty(input?.YoutubeChannelId))
                return new("Id do canal não informado!");

            if (string.IsNullOrEmpty(input.ChannelName))
                return new("Nome do canal não informado!");

            var existing = YoutubeChannelsDAO.FindOne(x => x.Id == input.ChannelId && x.AllyId == input.AllyId);

            if (YoutubeChannelsDAO.FindOne(x => x.YoutubeChannelId == input.YoutubeChannelId && x.Id != input.ChannelId && x.AllyId == input.AllyId) != null)
                return new("Canal já criado com este Id!");

            return string.IsNullOrEmpty(existing?.Id) ? YoutubeChannelsDAO.Insert(new(input)) : YoutubeChannelsDAO.Update(new(input, existing.Id));
        }

        public BaseApiOutput UpsertAllyChannel(HubYoutubeAllyChannelInput input)
        {
            if (string.IsNullOrEmpty(input?.AllyId))
                return new("Aliado não informado");

            if (!(input.ChannelId?.Any() ?? false))
                return new("Nenhum canal informado");

            if (!input.Linked)
            {
                var playlistsOnChannelIds = YoutubePlaylistDAO.Find(x => x.YoutubeChannelId == input.ChannelId).Select(x => x.Id);
                var highlightedPlaylist = YoutubeAllyPlaylistDAO.FindOne(x => playlistsOnChannelIds.Contains(x.PlaylistId) && x.AllyId == input.AllyId && x.Highlighted);
                if (highlightedPlaylist != null)
                {
                    highlightedPlaylist.Highlighted = false;
                    YoutubeAllyPlaylistDAO.Update(highlightedPlaylist);
                }
            }

            return input.Linked ? YoutubeAllyChannelDAO.InsertAllyChannel(input) : YoutubeAllyChannelDAO.RemoveAllyChannel(input.ChannelId, input.AllyId);
        }

        public BaseApiOutput RemovePlaylist(string id, string allyId, string userId)
        {
            if (string.IsNullOrEmpty(id))
                return new("Nenhuma playlist informada");

            var playlist = YoutubePlaylistDAO.FindOne(x => x.Id == id);

            if (playlist == null)
                return new("Nenhuma playlist encontrada");

            var user = HubUserDAO.FindById(userId);
            if (playlist.AllyId != allyId && !(user?.IsMasterAdmin ?? false))
                return new("Você não tem permissão para excluir essa playlist");

            LogHistoryDAO.Insert(new AppLogHistory
            {
                Message = "Playlist removida!",
                Type = AppLogTypeEnum.XApiInfo,
                Method = "RemoveChannel",
                Date = DateTime.Now,
                Username = user?.Name ?? userId,
                Data = JsonConvert.SerializeObject(playlist)
            });

            return YoutubePlaylistDAO.RemovePlaylist(id);
        }

        public BaseApiOutput RemoveChannel(string id, string allyId, string userId)
        {
            if (string.IsNullOrEmpty(id))
                return new("Nenhum canal informado");

            var channel = YoutubeChannelsDAO.FindOne(x => x.Id == id);

            if (channel == null)
                return new("Nenhum canal encontrada");

            var user = HubUserDAO.FindById(userId);
            if (channel.AllyId != allyId && !(user?.IsMasterAdmin ?? false))
                return new("Você não tem permissão para excluir esse canal");

            var playlistsIdsOnChannel = YoutubePlaylistDAO.Find(x => x.ChannelId == id).Select(x => x.Id);
            foreach (var Id in playlistsIdsOnChannel)
                YoutubePlaylistDAO.RemovePlaylist(Id);

            LogHistoryDAO.Insert(new AppLogHistory
            {
                Message = "Canal removido!",
                Type = AppLogTypeEnum.XApiInfo,
                Method = "RemoveChannel",
                Date = DateTime.Now,
                Username = user?.Name ?? userId,
                Data = JsonConvert.SerializeObject(channel)
            });

            return YoutubeChannelsDAO.RemoveChannel(id);
        }
    }
}
