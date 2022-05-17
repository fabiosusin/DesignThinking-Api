using Business.API.External.Wix;
using DAO.DBConnection;
using DAO.General.Log;
using DTO.General.Log.Database;
using DTO.General.Log.Enum;

namespace Business.AutomaticServices.Wix
{
    public class WixAutomaticRegisterPostService : BackgroundService
    {
        private readonly ILogger<WixAutomaticRegisterPostService> _logger;
        private readonly LogHistoryDAO LogHistoryDAO;
        private readonly BlWix BlWix;
        public WixAutomaticRegisterPostService(ILogger<WixAutomaticRegisterPostService> logger, XDataDatabaseSettings settings)
        {
            _logger = logger;
            LogHistoryDAO = new(settings);
            BlWix = new(settings);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                _logger.LogInformation("RegisterPostLoop running at: {time}", DateTimeOffset.Now);

                try
                {
                    _ = BlWix.RegisterPosts(DateTime.Now);
                    _ = BlWix.RegisterCategories();
                }
                catch (Exception ex)
                {
                    LogHistoryDAO.Insert(new AppLogHistory
                    {
                        ExceptionMessage = ex.Message,
                        Message = "Erro ao gerar Posts Automático!",
                        Type = AppLogTypeEnum.XApiExternalValidationError,
                        Method = "RegisterPostLoop",
                        Date = DateTime.Now
                    });
                }

                // 15 em 15 minutos
                await Task.Delay(900000, stoppingToken);
            }
        }
    }
}
