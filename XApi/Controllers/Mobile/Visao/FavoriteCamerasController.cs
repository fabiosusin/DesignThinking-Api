using Business.API.Mobile.Visao;
using DAO.DBConnection;
using DTO.Mobile.Visao.Input;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Mobile.Visao
{
    public class FavoriteCamerasController : VisaoBaseController<FavoriteCamerasController>
    {
        public FavoriteCamerasController(ILogger<FavoriteCamerasController> logger, XDataDatabaseSettings settings) : base(logger)
        {
            Bl = new(settings);
            BlFavoriteCamerasByMobileId = new(settings);
        }

        protected BlFavoriteCameras Bl;

        protected BlFavoriteCameras BlFavoriteCamerasByMobileId;
       
        [HttpPost, Route("favorite-cameras")]
        public IActionResult FavoriteCameras(AppFavoriteCameraListInput input) => Ok(Bl.GetFavoriteCamerasList(input));

        [HttpPost, Route("add-camera-bookmark")]
        public IActionResult AddCameraBookmark(AppCameraBookmarkInput input) => Ok(Bl.AddCameraBookmark(input));

        [HttpDelete, Route("remove-camera-bookmark")]
        public IActionResult RemoveCameraBookmark(string userMobileId, string cameraId, string allyId) => Ok(Bl.RemoveCameraBookmark(new AppCameraBookmarkInput(cameraId, userMobileId, allyId)));
    }
}
