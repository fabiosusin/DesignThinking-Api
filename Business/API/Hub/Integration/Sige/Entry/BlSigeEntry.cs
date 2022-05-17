using DAO.DBConnection;
using DAO.General.Log;
using DTO.General.Base.Api.Output;
using DTO.General.Log.Database;
using DTO.General.Log.Enum;
using DTO.Integration.Sige.Entry.Input;
using Newtonsoft.Json;
using Services.Integration.Sige.Entry;
using System;
using System.Threading.Tasks;
using Useful.Service;

namespace Business.API.Hub.Integration.Sige.Entry
{
    public class BlSigeEntry
    {
        protected SigeEntryService SigeEntryService;
        protected LogHistoryDAO LogHistoryDAO;
        public BlSigeEntry(XDataDatabaseSettings settings)
        {
            SigeEntryService = new();
            LogHistoryDAO = new(settings);
        }

        public async Task<BaseApiOutput> CreateEntry(decimal price, string documentNumber, string companyName, string customerName, string accountPlanName)
        {
            if (EnvironmentService.Get() == EnvironmentService.Dev)
                return new(true);

            var input = new SigeEntryInput(price, documentNumber, companyName, customerName, accountPlanName);
            try
            {
                return await SigeEntryService.CreateEntry(input).ConfigureAwait(false);
            }
            catch { return new(false, "Ocorreu um erro ao salvar o lançamento no ERP."); }
        }
    }
}
