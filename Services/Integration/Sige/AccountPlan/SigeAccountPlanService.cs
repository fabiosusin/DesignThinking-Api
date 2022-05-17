using DTO.Integration.Sige.AccountPlan.Input;
using DTO.Integration.Sige.AccountPlan.Output;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Integration.Sige.AccountPlan
{
    public class SigeAccountPlanService
    {
        internal SigeApiService SigeApiService;
        public SigeAccountPlanService() => SigeApiService = new();

        public async Task<IEnumerable<SigeAccountPlanOutput>> GetAccountPlans(SigeAccountPlanFiltersInput input = null) => await SigeApiService.GetAccountPlans(input);
    }
}
