using Business.API.Hub.Integration.Sige.AccountPlan;
using DAO.DBConnection;
using DAO.Hub.AccountPlan;
using DTO.General.Base.Api.Output;
using DTO.Integration.Sige.AccountPlan.Input;
using DTO.Hub.AccountPlan.Database;
using System.Linq;
using System.Threading.Tasks;

namespace Business.API.Hub.AccountPlan
{
    public class BlSyncAccountPlan
    {
        protected HubAccountPlanDAO AccountPlanDAO;
        protected BlSigeAccountPlan BlSigeAccountPlan;

        public BlSyncAccountPlan(XDataDatabaseSettings settings)
        {
            AccountPlanDAO = new(settings);
            BlSigeAccountPlan = new(settings);
        }

        public async Task<BaseApiOutput> SyncAccountPlan(SigeAccountPlanFiltersInput input)
        {
            var accountPlans = await BlSigeAccountPlan.GetAccountPlans(input).ConfigureAwait(false);
            if (!(accountPlans?.Any() ?? false))
                return new("Nenhum Plano de Conta encontrado");

            var existingAccountPlans = AccountPlanDAO.FindAll();
            foreach (var accountPlan in accountPlans)
            {
                var existing = existingAccountPlans?.FirstOrDefault(x => x.SigeId == accountPlan.Id);
                if (existing != null)
                {
                    existing.Name = accountPlan.Nome;
                    existing.Hierarchy = accountPlan.Hierarquia;
                    existing.Expense  = accountPlan.Despesa;
                    AccountPlanDAO.Update(existing);
                }
                else
                    AccountPlanDAO.Insert(new HubAccountPlan(accountPlan.Id, accountPlan.Nome, accountPlan.Hierarquia, accountPlan.Despesa));
            }

            return new(true);
        }
    }
}
