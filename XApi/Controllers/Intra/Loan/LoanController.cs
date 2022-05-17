using Business.API.Intra.Loan;
using DAO.DBConnection;
using DTO.API.Auth;
using DTO.Intra.Loan.Database;
using DTO.Intra.Loan.Input;
using DTO.Intra.Loan.Output;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace XApi.Controllers.Intra.Loan
{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Empréstimos - Intra"), Authorize(Policies.Bearer), Authorize(Policies.AppUser)]
    [Route("v1/Intra/[controller]")]
    public class LoanController : BaseController<LoanController>
    {
        public LoanController(ILogger<LoanController> logger, XDataDatabaseSettings settings) : base(logger) => Bl = new(settings);
        protected BlIntraLoan Bl;

        [HttpPost, Route("upsert-loan")]
        public IActionResult UpsertLoanEquipment(IntraLoan input) => Ok(Bl.UpsertLoan(input));

        [HttpPost, Route("equipment-devolution")]
        public IActionResult EquipmentDevolution(IntraLoanDetails input) => Ok(Bl.EquipmentDevolution(input));

        [HttpGet, Route("get-loan/{id}")]
        public IActionResult GetLoan(string id) => Ok(Bl.GetLoanDetails(id));

        [HttpPost, Route("list")]
        public IActionResult ListEquipment(IntraLoanListInput input) => Ok(Bl.List(input));
    }
}
