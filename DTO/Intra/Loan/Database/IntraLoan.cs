using DTO.General.Base.Database;
using System;
using System.Collections.Generic;

namespace DTO.Intra.Loan.Database
{
    public class IntraLoan : BaseData
    {
        public bool Returned { get; set; }
        public string UserId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DevolutionDate { get; set; }
        public List<string> EquipmentsIds { get; set; }
    }
}
