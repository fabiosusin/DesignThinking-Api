using DTO.General.Base.Api.Output;
using DTO.Integration.Sige.Customer.Input;
using DTO.Integration.Sige.Customer.Output;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Integration.Sige.Customer
{
    public class SigeCustomerService
    {
        internal SigeApiService SigeApiService;
        public SigeCustomerService() => SigeApiService = new();

        public async Task<BaseApiOutput> UpdateCustomer(SigeCustomerInput input) => await SigeApiService.UpdateCustomer(input);
        public async Task<List<SigeCustomerOutput>> GetCustomer(SigeCustomerFiltersInput input) => await SigeApiService.GetCustomer(input);
    }
}
