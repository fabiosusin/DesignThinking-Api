using DAO.DBConnection;
using DAO.General.Log;
using DTO.General.Log.Database;
using DTO.General.Log.Enum;
using DTO.Integration.Sige.AccountPlan.Input;
using DTO.Integration.Sige.AccountPlan.Output;
using Services.Integration.Sige.AccountPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.API.Hub.Integration.Sige.AccountPlan
{
    public class BlSigeAccountPlan
    {
        protected SigeAccountPlanService SigeAccountPlanService;
        protected LogHistoryDAO LogHistoryDAO;

        public BlSigeAccountPlan(XDataDatabaseSettings settings)
        {
            LogHistoryDAO = new(settings);
            SigeAccountPlanService = new();
        }

        public async Task<IEnumerable<SigeAccountPlanOutput>> GetAccountPlans(SigeAccountPlanFiltersInput input = null)
        {
            try
            {
                // O Sige não permite buscar por uma hierarquia específica, a unica forma de buscar a hierarquia 99
                // em diante é pulando os registros desnecessários além de add o limite para o máximo possível
                return (await SigeAccountPlanService.GetAccountPlans(input).ConfigureAwait(false))?.Where(x => x.Hierarquia.StartsWith("99")).ToList();
            }
            catch { return null; }
        }
    }
}
