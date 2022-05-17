using DAO.DBConnection;
using DAO.Hub.Application.Settings;
using DTO.General.Base.Api.Output;
using DTO.Hub.Application.AppSettings.Database;
using DTO.Hub.Application.AppSettings.Input;

namespace Business.API.Hub.Application.Settings
{
    public class BlHubAppSettings
    {
        protected AppSettingsDAO AppSettingsDAO;
        public BlHubAppSettings(XDataDatabaseSettings settings) => AppSettingsDAO = new(settings);

        public BaseApiOutput UpsertAppSettings(HubAppSettingsInput input)
        {
            if (input == null)
                return new("Requisição mal formada");

            if (string.IsNullOrEmpty(input.AllyId))
                return new("Id de Aliado não informado!");

            var existingId = string.Empty;
            if (string.IsNullOrEmpty(input.Id))
            {
                if (AppSettingsDAO.FindOne(x => x.AllyId == input.AllyId) != null)
                    return new("Configurações já cadastrada para este aliado!");
            }
            else
            {
                var existing = AppSettingsDAO.FindById(input.Id);
                if (string.IsNullOrEmpty(existing?.Id))
                    return new("Não possui configurações cadastradas para este aliado!");

                if (existing.AllyId != input.AllyId)
                    return new("Configuração não pertence a este aliado!");

                existingId = existing.Id;
            }

            if (string.IsNullOrEmpty(existingId))
                AppSettingsDAO.Insert(new AppSettings(input));
            else
                AppSettingsDAO.Update(new AppSettings(existingId, input));

            return new(true);
        }
    }
}
