using DAO.DBConnection;
using DTO.Integration.Surf.BaseDetails.Output;
using DTO.Integration.Surf.SMS.Input;
using System.Threading.Tasks;

namespace Services.Integration.Surf.Register.Customer
{
    public class SurfSMSService
    {
        internal SurfDetailsApiService SurfDetailsApiService;
        public SurfSMSService(XDataDatabaseSettings settings) => SurfDetailsApiService = new(settings);

        public async Task<SurfDetailsBaseApiOutput> SendSMS(SurfSendSmsOtpInput input) => await SurfDetailsApiService.SendSMS(input).ConfigureAwait(false);

        public async Task<SurfDetailsBaseApiOutput> ValidateSMSToken(SurfValidateSmsOtpInput input) => await SurfDetailsApiService.ValidateSMSToken(input).ConfigureAwait(false);
    }
}
