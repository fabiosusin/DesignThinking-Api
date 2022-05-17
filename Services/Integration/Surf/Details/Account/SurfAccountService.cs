using DAO.DBConnection;
using DTO.Integration.Surf.AccountDetails.Input;
using DTO.Integration.Surf.AccountDetails.Output;
using System.Threading.Tasks;

namespace Services.Integration.Surf.Register.Customer
{
    public class SurfAccountService
    {
        internal SurfDetailsApiService SurfDetailsApiService;
        public SurfAccountService(XDataDatabaseSettings settings) => SurfDetailsApiService = new(settings);

        public async Task<SurfAccountDetailsOutput> GetAccountDetails(SurfAccountDetailsInput input) => await SurfDetailsApiService.GetAccountDetails(input).ConfigureAwait(false);

        public async Task<SurfAccountDetailsOutput> GetAccountDetailsByCPF(SurfAccountDetailsCpfInput input) => await SurfDetailsApiService.GetAccountDetailsByCPF(input).ConfigureAwait(false);
    }
}
