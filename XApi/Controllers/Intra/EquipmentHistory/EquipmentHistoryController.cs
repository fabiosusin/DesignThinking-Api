using Business.API.Intra.EquipmentHistory;
using DAO.DBConnection;
using DTO.API.Auth;
using DTO.Intra.EquipamentHistory.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Intra.EquipmentHistory
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Históricos de Equipamentos - Intra"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Intra/[controller]")]
    public class EquipmentHistoryController : BaseController<EquipmentHistoryController>
    {
        public EquipmentHistoryController(ILogger<EquipmentHistoryController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);
        protected BlIntraEquipmentHistory Bl;

        [HttpPost, Route("list")]
        public IActionResult ListEquipment(IntraEquipmentHistoryListInput input) => Ok(Bl.List(input));
    }
}
