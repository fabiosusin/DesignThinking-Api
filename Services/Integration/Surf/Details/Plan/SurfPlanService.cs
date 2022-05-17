using DAO.DBConnection;
using DTO.Integration.Surf.AccountPlan.Input;
using DTO.Integration.Surf.AccountPlan.Output;
using System.Threading.Tasks;

namespace Services.Integration.Surf.Register.Customer
{
    public class SurfPlanService
    {
        internal SurfDetailsApiService SurfDetailsApiService;
        public SurfPlanService(XDataDatabaseSettings settings) => SurfDetailsApiService = new(settings);

        public async Task<SurfBundleInfoOutput> GetBundleInfo(SurfBundleInfoInput input) => await SurfDetailsApiService.GetBundleInfo(input).ConfigureAwait(false);

        public async Task<SurfFreeUsageInfoOutput> GetFreeUsageInfo(SurfFreeUsageInfoInput input) => await SurfDetailsApiService.GetFreeUsageInfo(input).ConfigureAwait(false);
    }
}
