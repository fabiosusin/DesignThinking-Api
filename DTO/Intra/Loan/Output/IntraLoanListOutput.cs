using DTO.General.Base.Api.Output;
using DTO.Intra.Loan.Database;
using System;
using System.Collections.Generic;

namespace DTO.Intra.Loan.Output
{
    public class IntraLoanListOutput : BaseApiOutput
    {
        public IntraLoanListOutput(string msg) : base(msg) { }
        public IntraLoanListOutput(IEnumerable<IntraLoanListData> loans) : base(true) => Loans = loans;
        public IEnumerable<IntraLoanListData> Loans { get; set; }
    }

    public class IntraLoanListData
    {
        public IntraLoanListData(IntraLoan loan, string employeeName, List<string> equipmentName)
        {
            EmployeeName = employeeName;
            EquipmentName = equipmentName;

            if (loan == null)
                return;

            Id = loan.Id;
            LoanDate = loan.LoanDate;
            DevolutionDate = loan.DevolutionDate;
            Returned = loan.Returned;
        }
        public string Id { get; set; }
        public string EmployeeName { get; set; }
        public List<string> EquipmentName { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DevolutionDate { get; set; }
        public bool Returned { get; set; }
    }
}
