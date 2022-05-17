using Business.API.Mobile.Visao;
using DAO.DBConnection;
using DTO.Mobile.Visao.Enum;
using DTO.Mobile.Visao.Input;
using DTO.Mobile.Visao.Output;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace XApi.Controllers.Mobile.Visao
{
    [ApiController]
    public class CamerasController : VisaoBaseController<CamerasController>
    {
        public CamerasController(ILogger<CamerasController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);

        protected BlCameras Bl;

        [HttpPost, Route("get-cameras")]
        public IActionResult GetCameras(AppCameraListInput input) => Ok(Bl.GetCamerasList(input));

        [HttpGet, Route("get-camera-details")]
        public IActionResult GetCameraDetails(string id, string mobileId, string allyId, bool othersWithFreeAccess) => Ok(Bl.GetCameraDetails(id, mobileId, allyId, othersWithFreeAccess));

        [HttpGet, Route("get-featured-cities")]
        public IActionResult GetFeaturedCities(string allyId, bool othersWithFreeAccess) => Ok(Bl.GetCities(allyId, othersWithFreeAccess, AppFindCitiesTypeEnum.Featured));

        [HttpGet, Route("get-camera-cities")]
        public IActionResult GetCameraCities(string allyId, bool othersWithFreeAccess) => Ok(Bl.GetCities(allyId, othersWithFreeAccess, AppFindCitiesTypeEnum.All));
    }
}
