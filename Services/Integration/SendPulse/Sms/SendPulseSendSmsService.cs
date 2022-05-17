using DAO.DBConnection;
using DTO.Integration.SendPulse.SMS.Input;
using DTO.Integration.SendPulse.SMS.Output;
using System.Threading.Tasks;

namespace Services.Integration.SendPulse.Sms
{
    public class SendPulseSendSmsService
    {
        internal SendPulseApiService SendPulseApiService;
        public SendPulseSendSmsService(XDataDatabaseSettings settings) => SendPulseApiService = new(settings);

        public async Task<SendSmsResultOutput> SendSms(SendSmsInput input) => await SendPulseApiService.SendSmsOutput(input).ConfigureAwait(false);
    }
}
