using Business.API.Hub.Product;
using DAO.DBConnection;
using DTO.API.Auth;
using DTO.Hub.Product.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace XApi.Controllers.Hub.Product
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Produto"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Hub/Product")]
    public class ProductController : BaseController<ProductController>
    {
        protected BlProduct BlProduct;
        protected BlSyncProduct BlSync;
        public ProductController(ILogger<ProductController> logger, XDataDatabaseSettings settings) : base(logger)
        {
            BlProduct = new(settings);
            BlSync = new(settings);
        }

        [HttpPost, Route("list")]
        public IActionResult List(HubProductListInput input) => Ok(BlProduct.List(input));

        [HttpPost, Route("order-product-list")]
        public IActionResult OrderProductList(HubProductListInput input) => Ok(BlProduct.OrderProductList(input));

        [HttpGet, Route("sync")]
        public async Task<IActionResult> SyncGetProducts() => Ok(await BlSync.GetProducts().ConfigureAwait(false));

        [HttpGet, Route("category-list")]
        public IActionResult ProductCategoryList(string allyId) => Ok(BlProduct.ProductCategoryList(allyId));

        [HttpGet, Route("get-all-categories")]
        public IActionResult GetAllProductCategories() => Ok(BlProduct.GetAllProductCategories());
    }
}
