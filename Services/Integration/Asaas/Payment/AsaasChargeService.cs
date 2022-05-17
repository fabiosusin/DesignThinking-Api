using DTO.Integration.Asaas.Payment.Output;
using DTO.Integration.Asaas.Payments.Input;
using DTO.Integration.Asaas.Payments.Output;
using Services.Integration.Sige;
using System.Threading.Tasks;

namespace Services.Integration.Asaas.Customer
{
    public class AsaasChargeService
    {
        internal AsaasApiService AsaasApiService;
        public AsaasChargeService() => AsaasApiService = new();

        public async Task<AsaasCreateChargeResultOutput> CreateCharge(AsaasCreateChargeInput input) => await AsaasApiService.CreateCharge(input);

        public async Task<AsaasCreateChargeResultOutput> GetChargeDetails(string input) => await AsaasApiService.GetChargeDetails(input);

        public async Task<AsaasGetQrCodeResultOutput> GetQrCode(string input) => await AsaasApiService.GetQrCode(input);
        
        public async Task<AsaasCancelPaymentResultOutput> CancelCharge(string input) => await AsaasApiService.CancelCharge(input);
    }
}
