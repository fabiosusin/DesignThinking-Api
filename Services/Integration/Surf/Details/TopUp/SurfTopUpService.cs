using DAO.DBConnection;
using DTO.Integration.Surf.TopUp.Input;
using DTO.Integration.Surf.TopUp.Output;
using System.Threading.Tasks;

namespace Services.Integration.Surf.Register.Customer
{
    public class SurfTopUpService
    {
        internal SurfDetailsApiService SurfDetailsApiService;
        public SurfTopUpService(XDataDatabaseSettings settings) => SurfDetailsApiService = new(settings);

        public async Task<SurfTopUpHistoryOutput> GetTopUpHistory(SurfTopUpHistoryInput input) => await SurfDetailsApiService.GetTopUpHistory(input).ConfigureAwait(false);
    }
}
