using DAO.DBConnection;
using DAO.General.Log;
using DTO.General.Log.Database;
using DTO.General.Log.Enum;
using DTO.Integration.Surf.Operator.Output;
using Services.Integration.Surf.Register.Operator;
using System;
using System.Threading.Tasks;

namespace Business.API.Hub.Integration.Surf.Operator
{
    public class BlSurfOperator
    {
        protected SurfOperatorService SurfOperatorService;
        protected LogHistoryDAO LogHistoryDAO;
        public BlSurfOperator(XDataDatabaseSettings settings)
        {
            LogHistoryDAO = new(settings);
            SurfOperatorService = new();
        }

        public async Task<SurfOperatorOutput> GetOperators()
        {
            try
            {
                return await SurfOperatorService.GetOperators().ConfigureAwait(false);
            }
            catch { return null; }
        }
    }
}
