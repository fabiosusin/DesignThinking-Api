using DTO.General.Base.Api.Output;
using DTO.Hub.AccountPlan.Database;
using System.Collections.Generic;

namespace DTO.Hub.AccountPlan.Output
{
    public class HubAccountPlanListOutput: BaseApiOutput
    {
        public HubAccountPlanListOutput(string msg) : base(msg) { }
        public HubAccountPlanListOutput(IEnumerable<HubAccountPlan> plans) : base(true) => AccountPlans = plans;
        public IEnumerable<HubAccountPlan> AccountPlans { get; set; }
    }
}
