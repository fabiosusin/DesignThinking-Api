using DTO.Integration.Surf.Customer.Output;
using DTO.Integration.Surf.Input.Customer;
using System.Threading.Tasks;

namespace Services.Integration.Surf.Register.Customer
{
    public class SurfCustomerService
    {
        internal SurfApiService SurfApiService;
        public SurfCustomerService() => SurfApiService = new();

        public async Task<SurfCustomerOutput> AddCustomer(SurfCustomerInput input) => await SurfApiService.AddCustomer(input).ConfigureAwait(false);

        public async Task<SurfCustomerOutput> GetCustomer(string input) => await SurfApiService.GetCustomer(input).ConfigureAwait(false);
    }
}
