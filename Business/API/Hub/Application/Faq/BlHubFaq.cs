using DAO.DBConnection;
using DAO.General.Log;
using DAO.Hub.Application.Faq;
using DAO.Hub.UserDAO;
using DTO.General.Base.Api.Output;
using DTO.General.Log.Database;
using DTO.General.Log.Enum;
using DTO.Hub.Application.Faq.Input;
using Newtonsoft.Json;
using System;

namespace Business.API.Hub.Application.Faq
{
    public class BlHubFaq
    {
        private readonly AllyFaqDAO FaqDAO;
        private readonly LogHistoryDAO LogHistoryDAO;
        private readonly HubUserDAO HubUserDAO;

        public BlHubFaq(XDataDatabaseSettings settings)
        {
            FaqDAO = new(settings);
            LogHistoryDAO = new(settings);
            HubUserDAO = new(settings);
        }

        public BaseApiOutput UpsertAppFaq(HubAppFaqInput input)
        {
            if (string.IsNullOrEmpty(input.Question))
                return new("Pergunta não informada!");

            if (string.IsNullOrEmpty(input.Answer))
                return new("Reposta não informada!");

            if (string.IsNullOrEmpty(input.AllyId))
                return new("Id do aliado não informado!");

            var existing = FaqDAO.FindOne(x => x.Id == input.Id);

            return string.IsNullOrEmpty(existing?.Id) ? FaqDAO.Insert(new(input)) : FaqDAO.Update(new(input, existing.Id));
        }

        public BaseApiOutput RemoveAppFaq(HubAppFaqInput input)
        {
            if (string.IsNullOrEmpty(input.Id))
                return new("Nenhuma pergunta informada");

            var faq = FaqDAO.FindOne(x => x.Id == input.Id);

            if (faq == null)
                return new("Nenhuma pergunta encontrada");

            if (string.IsNullOrEmpty(input.AllyId))
                return new("Nenhum aliado informado");

            var user = HubUserDAO.FindById(input.UserId);
            if (faq.AllyId != input.AllyId && !(user?.IsMasterAdmin ?? false))
                return new("Você não tem permissão para excluir essa pergunta");

            LogHistoryDAO.Insert(new AppLogHistory
            {
                Message = "Faq removida!",
                Type = AppLogTypeEnum.XApiInfo,
                Method = "RemoveAppFaq",
                Date = DateTime.Now,
                Username = user?.Name ?? input.UserId,
                Data = JsonConvert.SerializeObject(faq)
            });

            return FaqDAO.RemoveById(faq.Id);
        }

    }
}
