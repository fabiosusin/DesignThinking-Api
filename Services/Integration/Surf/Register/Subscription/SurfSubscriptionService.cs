using DTO.Integration.Surf.Subscription.Input;
using DTO.Integration.Surf.Subscription.Output;
using System.Threading.Tasks;

namespace Services.Integration.Surf.Register.Subscription
{
    public class SurfSubscriptionService
    {
        internal SurfApiService SurfApiService;
        public SurfSubscriptionService()
        {
            SurfApiService = new();
        }

        public async Task<SurfSubscriptionOutput> AddSubscription(SurfSubscriptionInput input) => await SurfApiService.AddSubscription(input).ConfigureAwait(false);
    }
}
