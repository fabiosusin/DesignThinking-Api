using Business.API.Hub.Integration.Sige.Order;
using Business.API.Hub.Integration.Surf.Recurrence;
using DAO.DBConnection;
using DAO.General.Log;
using DTO.General.Log.Database;
using DTO.General.Log.Enum;

namespace AutomaticServices.Services.Surf
{
    public class SurfAutomaticRecurrenceService : BackgroundService
    {
        private readonly ILogger<SurfAutomaticRecurrenceService> _logger;
        private readonly BlSurfRecurrence BlSurfRecurrence;
        private readonly LogHistoryDAO LogHistoryDAO;
        public SurfAutomaticRecurrenceService(ILogger<SurfAutomaticRecurrenceService> logger, XDataDatabaseSettings settings)
        {
            _logger = logger;
            LogHistoryDAO = new(settings);
            BlSurfRecurrence = new(settings);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                _logger.LogInformation("RegisterRecurrenceLoop running at: {time}", DateTimeOffset.Now);

                try
                {
                    LogHistoryDAO.Insert(new AppLogHistory
                    {
                        Message = "Gerando Recorrência Automática!",
                        Type = AppLogTypeEnum.XApiInfo,
                        Method = "RegisterRecurrenceLoop",
                        Date = DateTime.Now
                    });

                    _ = BlSurfRecurrence.RegisterRecurrence(DateTime.Now.Date);
                }
                catch (Exception ex)
                {
                    LogHistoryDAO.Insert(new AppLogHistory
                    {
                        ExceptionMessage = ex.Message,
                        Message = "Erro ao gerar Recorrência Automática!",
                        Type = AppLogTypeEnum.XApiHubValidationError,
                        Method = "RegisterRecurrenceLoop",
                        Date = DateTime.Now
                    });
                }

                // 1 vez ao dia
                await Task.Delay(86400000, stoppingToken);
            }
        }
    }
}
