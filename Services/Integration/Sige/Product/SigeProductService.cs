using DTO.Integration.Sige.Product;
using DTO.Integration.Sige.Product.Output;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Integration.Sige.Product
{
    public class SigeProductService
    {
        internal SigeApiService SigeApiService;
        public SigeProductService() => SigeApiService = new();

        public async Task<IEnumerable<SigeProductInput>> GetProducts() => await SigeApiService.GetProducts();
    }
}
