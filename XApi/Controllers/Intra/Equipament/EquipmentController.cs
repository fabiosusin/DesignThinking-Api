using Business.API.Hub.Equipament;
using DAO.DBConnection;
using DTO.API.Auth;
using DTO.General.Image.Input;
using DTO.Intra.Equipament.Database;
using DTO.Intra.Equipament.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Intra.Equipment
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Equipamentos - Intra"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Intra/[controller]")]
    public class EquipmentController : BaseController<EquipmentController>
    {
        public EquipmentController(ILogger<EquipmentController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);
        protected BlIntraEquipment Bl;

        [HttpGet, Route("get-equipment/{id}")]
        public IActionResult GetEquipment(string id) => Ok(Bl.GetById(id));

        [HttpGet, Route("get-equipment-images/{id}/{size}")]
        public IActionResult GetEquipmentImages(string id, ListResolutionsSize size) => Ok(Bl.GetEquipmentImages(id, size));

        [HttpPost, Route("upsert-equipment")]
        public IActionResult UpsertEquipment(IntraEquipment input) => Ok(Bl.UpsertEquipment(input));

        [HttpDelete, Route("delete/{id}")]
        public IActionResult DeleteEquipament(string id) => Ok(Bl.DeleteEquipament(id));

        [HttpPost, Route("list")]
        public IActionResult ListEquipment(IntraEquipmentListInput input) => Ok(Bl.List(input));
    }
}
