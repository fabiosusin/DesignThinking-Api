using DAO.DBConnection;
using DTO.Integration.Surf.Subscriber.Input;
using DTO.Integration.Surf.Subscriber.Output;
using DTO.Surf.Enum;
using Services.Integration.Surf.Register.Customer;
using System.Threading.Tasks;
using Useful.Extensions;

namespace Business.API.Mobile.Surf
{
    public class BlSubscriberDetails
    {
        private SurfSubscriberService SurfSubscriberService;
        public BlSubscriberDetails(XDataDatabaseSettings settings)
        {
            SurfSubscriberService = new(settings);
        }
        public async Task<SurfSubscriberDetailsOutput> SubscriberInformation(string msisdn) => string.IsNullOrEmpty(msisdn) ?
                    new SurfSubscriberDetailsOutput
                    {
                        CodeStr = AppReturnCodesEnum.P03.GetDescription(),
                        Msg = "MSISDN não informado!"
                    } : await SurfSubscriberService.GetSubscriberInformation(new SurfSubscriberDetailsInput(msisdn)).ConfigureAwait(false);
    }
}
