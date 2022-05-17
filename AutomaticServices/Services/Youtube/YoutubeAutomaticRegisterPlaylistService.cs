using Business.API.External.Youtube;
using DAO.DBConnection;
using DAO.General.Log;
using DTO.General.Log.Database;
using DTO.General.Log.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomaticServices.Services.Youtube
{
    public class YoutubeAutomaticRegisterPlaylistService : BackgroundService
    {
        private readonly ILogger<YoutubeAutomaticRegisterPlaylistService> _logger;
        private readonly BlYoutube BlYoutube;
        private readonly LogHistoryDAO LogHistoryDAO;
        public YoutubeAutomaticRegisterPlaylistService(ILogger<YoutubeAutomaticRegisterPlaylistService> logger, XDataDatabaseSettings settings)
        {
            _logger = logger;
            LogHistoryDAO = new(settings);
            BlYoutube = new(settings);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                _logger.LogInformation("RegisterPlaylistsLoop running at: {time}", DateTimeOffset.Now);

                try
                {
                    _ = BlYoutube.SavePlaylists();
                }
                catch (Exception ex)
                {
                    LogHistoryDAO.Insert(new AppLogHistory
                    {
                        ExceptionMessage = ex.Message,
                        Message = "Erro ao gerar Playlists Automática!",
                        Type = AppLogTypeEnum.XApiExternalValidationError,
                        Method = "RegisterPlaylistsLoop",
                        Date = DateTime.Now
                    });
                }

                // a cada 1 hora
                await Task.Delay(1000 * 60 * 60, stoppingToken);
            }
        }
    }
}
