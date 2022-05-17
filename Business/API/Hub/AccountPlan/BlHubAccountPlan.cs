using DAO.DBConnection;
using DAO.Hub.AccountPlan;
using DTO.General.Base.Api.Output;
using DTO.Hub.AccountPlan.Input;
using DTO.Hub.AccountPlan.Output;
using System.Linq;

namespace Business.API.Hub.AccountPlan
{
    public class BlHubAccountPlan
    {
        protected HubAccountPlanDAO AccountPlanDAO;

        public BlHubAccountPlan(XDataDatabaseSettings settings)
        {
            AccountPlanDAO = new(settings);
        }

        public HubAccountPlanListOutput List(HubAccountPlanListInput input)
        {
            var plans = AccountPlanDAO.List(input);
            if (!(plans?.Any() ?? false))
                return new("Nenhum Plano de Contas encontrado!");

            return new(plans);
        }
    }
}
