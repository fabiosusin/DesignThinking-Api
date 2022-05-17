using DTO.Integration.Asaas.Customer.Input;
using DTO.Integration.Asaas.Customer.Output;
using Services.Integration.Sige;
using System.Threading.Tasks;

namespace Services.Integration.Asaas.Customer
{
    public class AsaasCustomerService
    {
        internal AsaasApiService AsaasApiService;
        public AsaasCustomerService() => AsaasApiService = new();

        public async Task<AsaasGetCustomerOutput> CreateCustomer(AsaasCreateCustomerInput input) => await AsaasApiService.CreateCustomer(input);
        public async Task<AsaasGetCustomerOutput> GetCustomer(string input) => await AsaasApiService.GetCustomer(input);
        public async Task<AsaasListCustomersResultOutput> ListCustomers(AsaasFilterCustomers input) => await AsaasApiService.ListCustomers(input);
    }
}
