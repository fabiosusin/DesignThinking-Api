using Business.API.External.Surf;
using DAO.DBConnection;
using DTO.General.Surf.Input;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Threading.Tasks;
using Useful.Extensions.FilesExtension;

namespace XApi.Controllers.External.Surf
{
    [ApiController]
    public class DeaflympicsController : SurfBaseController<DeaflympicsController>
    {
        public DeaflympicsController(ILogger<DeaflympicsController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);

        protected BlDeaflympics Bl;

        [HttpPost, Route("register-chip")]
        public async Task<IActionResult> RegisterCameras(SurfDeaflympicsRegisterChipsInput input) => File(Encoding.ASCII.GetBytes(await Bl.RegisterChips(input).ConfigureAwait(false)), FilesExtension.GetContentType(".csv"));
    }
}
