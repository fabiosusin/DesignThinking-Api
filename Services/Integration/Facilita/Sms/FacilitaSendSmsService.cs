using DTO.Integration.Facilita.SMS.Output;
using DTO.Integration.SendPulse.SMS.Input;
using System.Threading.Tasks;

namespace Services.Integration.Facilita.Sms
{
    public class FacilitaSendSmsService
    {
        internal FacilitaApiService FacilitaApiService;
        public FacilitaSendSmsService() => FacilitaApiService = new();

        public async Task<FacilitaSendSmsResultOutput> SendSms(SendSmsInput input) => await FacilitaApiService.SendSmsOutput(input).ConfigureAwait(false);
    }
}
