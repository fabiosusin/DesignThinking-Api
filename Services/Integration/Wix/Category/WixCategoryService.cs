using DTO.Integration.Wix.Category.Output;
using System.Threading.Tasks;

namespace Services.Integration.Wix.Category
{
    public class WixCategoryService
    {
        internal WixApiService WixApiService;

        public WixCategoryService() => WixApiService = new();

        public async Task<GetWixCategoryListOutput> GetCategories(int? offset) => await WixApiService.GetCategories(offset).ConfigureAwait(false);

        public async Task<WixCategoryDataOutput> GetCategoryBySlug(string input) => (await WixApiService.GetCategoryBySlug(input).ConfigureAwait(false))?.Category;

        public async Task<WixCategoryDataOutput> GetCategoryById(string input) => (await WixApiService.GetCategoryById(input).ConfigureAwait(false))?.Category;
    }
}
