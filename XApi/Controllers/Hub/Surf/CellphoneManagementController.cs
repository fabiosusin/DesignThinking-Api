using Business.API.Hub.Integration.Surf.Base;
using Business.API.Hub.Integration.Surf.Recurrence;
using DAO.DBConnection;
using DTO.Hub.Cellphone.Database;
using DTO.Hub.Cellphone.Input;
using DTO.Integration.Surf.Recurrence.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace XApi.Controllers.Hub.Surf
{
    [ApiController]
    public class CellphoneManagementController : SurfBaseController<CellphoneManagementController>
    {
        protected BlBaseSurf BlBaseSurf;
        protected BlSurfRecurrence BlSurfRecurrence;
        public CellphoneManagementController(ILogger<CellphoneManagementController> logger, XDataDatabaseSettings settings) : base(logger)
        {
            BlBaseSurf = new(settings);
            BlSurfRecurrence = new(settings);
        }

        [HttpGet, Route("{managementId}")]
        public IActionResult GetManagement(string managementId) => Ok(BlBaseSurf.Get(managementId));

        [HttpGet, Route("details/{managementId}")]
        public IActionResult GetManagementDetails(string managementId) => Ok(BlBaseSurf.GetCellphoneDetails(managementId));

        [HttpPost, Route("send-subscription")]
        public async Task<IActionResult> SendSubscriptionAsync(HubCellphoneManagementStepOneInput input) => Ok(await BlBaseSurf.AddSubscription(input).ConfigureAwait(false));

        [HttpPost, Route("update")]
        public IActionResult Update(HubCellphoneManagement management) => Ok(BlBaseSurf.Update(management));

        [HttpPost, Route("cancel-recurrence")]
        public async Task<IActionResult> CancelRecurrence(HubCellphoneRecurrenceChangeInput input) => Ok(await BlBaseSurf.ChangeRecurrenceStatus(input, SurfRecurrenceStatus.Cancel).ConfigureAwait(false));

        [HttpPost, Route("active-recurrence")]
        public async Task<IActionResult> ActiveRecurrence(HubCellphoneRecurrenceChangeInput input) => Ok(await BlBaseSurf.ChangeRecurrenceStatus(input, SurfRecurrenceStatus.Paid).ConfigureAwait(false));

        [HttpPost, Route("reprocess-management/{managementId}/{allyId}")]
        public async Task<IActionResult> ReprocessCellphoneManagement(string managementId, string allyId) => Ok(await BlBaseSurf.ReprocessCellphoneManagement(managementId, allyId).ConfigureAwait(false));

        [HttpPost, Route("register-recurrence")]
        public async Task<IActionResult> RegisterRecurrence(DateTime input)
        {
            await BlSurfRecurrence.RegisterRecurrence(input).ConfigureAwait(false);
            return Ok("Registro de Recorrência finalizado com sucesso!");
        }

        [HttpPost, Route("list")]
        public IActionResult ListUser(HubRecurrenceListInput input) => Ok(BlBaseSurf.List(input));
    }
}
