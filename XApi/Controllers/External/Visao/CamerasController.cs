using Business.API.External.Visao;
using DAO.DBConnection;
using DTO.External.Visao.Input;
using DTO.General.Pagination.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace XApi.Controllers.External.Visao
{
    [ApiController]
    public class CamerasController : VisaoBaseController<CamerasController>
    {
        public CamerasController(ILogger<CamerasController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);

        protected BlCameras Bl;

        [HttpGet, Route("get-camera")]
        public IActionResult GetCameras(string id, string allyId) => Ok(Bl.GetCamera(new CameraListExternalInput(new FiltersCameraExternalInput(id, allyId), new PaginatorInput(1, 1))));

        [HttpPost, Route("get-cameras")]
        public IActionResult GetCameras(CameraListExternalInput input) => Ok(Bl.GetCamerasList(input));

        [HttpPost, Route("register-camera")]
        public IActionResult RegisterCamera(VisaoCameraInput input) => Ok(Bl.RegisterCamera(input));

        [HttpPost, Route("register-cameras")]
        public IActionResult RegisterCameras(List<VisaoCameraInput> input) => Ok(Bl.RegisterCameras(input).ToList());

        [HttpDelete, Route("delete-camera")]
        public IActionResult DeleteCamera(string cameraId) => Ok(Bl.DeleteCamera(cameraId));

        [HttpPut, Route("update-camera")]
        public IActionResult UpdateCamera(VisaoCameraInput input) => Ok(Bl.UpdateCamera(input));

    }
}
