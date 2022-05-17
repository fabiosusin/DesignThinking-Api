using DTO.Integration.Surf.Plan.Output;
using System.Threading.Tasks;

namespace Services.Integration.Surf.Register.Plan
{
    public class SurfPlanService
    {
        internal SurfApiService SurfApiService;
        public SurfPlanService() => SurfApiService = new();

        public async Task<SurfPlanOutput> GetPlans()
        {
            try
            {
                return await SurfApiService.GetPlans().ConfigureAwait(false);
            }
            catch
            {
                return null;
            }
        }
    }
}
