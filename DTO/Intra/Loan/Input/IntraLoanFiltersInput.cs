using System;
using System.Collections.Generic;

namespace DTO.Intra.Loan.Input
{
    public class IntraLoanFiltersInput
    {
        public string UserId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public List<string> EquipmentIds { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public LoanDateFilterEnum DateFilterType { get;set;}
        public LoanReturnedFilterEnum ReturnedFilterType { get; set; }
    }
    public enum LoanDateFilterEnum
    {
        Unknown,
        LoanDate,
        DevolutionDate
    }
    public enum LoanReturnedFilterEnum
    {
        Unknown,
        Returned,
        NotReturned
    }
}
