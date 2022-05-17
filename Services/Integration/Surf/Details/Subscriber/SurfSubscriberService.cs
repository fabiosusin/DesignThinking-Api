using DAO.DBConnection;
using DTO.Integration.Surf.Subscriber.Input;
using DTO.Integration.Surf.Subscriber.Output;
using System.Threading.Tasks;

namespace Services.Integration.Surf.Register.Customer
{
    public class SurfSubscriberService
    {
        internal SurfDetailsApiService SurfDetailsApiService;
        public SurfSubscriberService(XDataDatabaseSettings settings) => SurfDetailsApiService = new(settings);

        public async Task<SurfSubscriberDetailsOutput> GetSubscriberInformation(SurfSubscriberDetailsInput input) => await SurfDetailsApiService.GetSubscriberInformation(input).ConfigureAwait(false);
    }
}
