using DTO.Integration.Sige.Order.Input;
using DTO.Integration.Sige.Order.Output;
using System.Threading.Tasks;

namespace Services.Integration.Sige.Order
{
    public class SigeOrderService
    {
        internal SigeApiService SigeApiService;
        public SigeOrderService() => SigeApiService = new();

        public async Task<SigeOrderApiOutput> CreateOrder(SigeOrderInput input) => await SigeApiService.CreateOrder(input);
    }
}
