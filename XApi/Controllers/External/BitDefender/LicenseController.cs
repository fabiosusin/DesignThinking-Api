using Business.API.External.BitDefender;
using DAO.DBConnection;
using DTO.External.BitDefender.Input;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace XApi.Controllers.External.BitDefender
{
    [ApiController]
    public class LicenseController : BitDefenderBaseController<LicenseController>
    {
        public LicenseController(ILogger<LicenseController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);

        protected BlLicense Bl;

        [HttpPost, Route("register-licenses")]
        public IActionResult RegisterCameras(List<BitDefenderLicenseDataInputApi> input) => Ok(Bl.RegisterLicenses(input).ToList());
    }
}
