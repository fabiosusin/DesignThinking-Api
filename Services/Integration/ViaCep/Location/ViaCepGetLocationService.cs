using DTO.General.Address.Output;
using System.Threading.Tasks;

namespace Services.Integration.ViaCep.Location
{
    public class ViaCepGetLocationService
    {
        internal ViaCepApiService ViaCepApiService;
        public ViaCepGetLocationService() => ViaCepApiService = new();

        public async Task<ViaCepAddressOutput> GetAddress(string input) => await ViaCepApiService.GetAddress(input).ConfigureAwait(false);
    }
}
