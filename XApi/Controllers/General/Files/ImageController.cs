using Business.API.Mobile.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Web;

namespace XApi.Controllers.General.Files
{
    [ApiController, AllowAnonymous]
    public class ImageController : FilesBaseController<ImageController>
    {
        public ImageController(ILogger<ImageController> logger) : base(logger) => Bl = new BlImage();

        protected BlImage Bl;

        [HttpGet, Route("get-image")]
        public IActionResult GetImage(string filePath, string contentType) => File(BlImage.GetFile(filePath), contentType);

        [HttpPost, Route("remove-image")]
        public IActionResult RemoveImage(string filePath, string contentType) => File(System.IO.File.ReadAllBytes(HttpUtility.UrlDecode(filePath)), contentType);

    }
}
