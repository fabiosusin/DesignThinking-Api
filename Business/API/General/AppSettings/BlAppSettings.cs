using DAO.DBConnection;
using DAO.Hub.Application.Settings;
using DTO.Hub.Application.AppSettings.Output;

namespace Business.API.General.BlAppSettings
{
    public class BlAppSettings
    {
        protected AppSettingsDAO AppSettingsDAO;
        public BlAppSettings(XDataDatabaseSettings settings) => AppSettingsDAO = new(settings);

        public HubAppSettingsDetailsOutput GetAppSettings(string allyId)
        {
            if (string.IsNullOrEmpty(allyId))
                return new("Id de Aliado não informado");

            var settings = AppSettingsDAO.FindOne(x => x.AllyId == allyId);
            return settings == null ? new("Configurações não encontradas para este aliado!") : new(settings);
        }
    }
}
