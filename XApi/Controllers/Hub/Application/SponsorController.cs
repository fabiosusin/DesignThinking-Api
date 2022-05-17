using Business.API.General.AppSponsor;
using Business.API.Hub.Application.Sponsor;
using DAO.DBConnection;
using DTO.Hub.Application.Sponsor.Input;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Hub.Application
{
    [ApiController]
    public class SponsorController : ApplicationBaseController<SponsorController>
    {
        protected BlHubAppSponsor BlHubAppSponsor;
        protected BlAppSponsor BlAppSponsor;
        public SponsorController(ILogger<SponsorController> logger, XDataDatabaseSettings settings) : base(logger)
        {
            BlHubAppSponsor = new(settings);
            BlAppSponsor = new (settings);
        }

        [HttpPost, Route("list")]
        public IActionResult ListSponsor(HubAppSponsorListInput input) => Ok(BlAppSponsor.List(input));

        [HttpPost, Route("upsert-app-sponsor")]
        public IActionResult UpsertAppSponsor(HubAppSponsorInput input) => Ok(BlHubAppSponsor.UpsertAppSponsor(input));
    }
}
