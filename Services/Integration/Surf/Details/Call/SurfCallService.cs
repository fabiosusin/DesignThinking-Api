using DAO.DBConnection;
using DTO.Integration.Surf.Call.Input;
using DTO.Integration.Surf.Call.Output;
using System.Threading.Tasks;

namespace Services.Integration.Surf.Register.Customer
{
    public class SurfCallService
    {
        internal SurfDetailsApiService SurfDetailsApiService;
        public SurfCallService(XDataDatabaseSettings settings) => SurfDetailsApiService = new(settings);

        public async Task<SurfCallHistoryOutput> GetCallHistory(SurfCallHistoryInput input) => await SurfDetailsApiService.GetCallHistory(input).ConfigureAwait(false);
    }
}
