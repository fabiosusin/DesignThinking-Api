using DTO.General.Base.Database;
using DTO.Intra.Equipament.Database;
using DTO.Intra.Loan.Database;
using System;

namespace DTO.Intra.LoanHistory.Database
{
    public class IntraEquipmentHistory : BaseData
    {
        public IntraEquipmentHistory() { }
        public IntraEquipmentHistory(IntraLoan loan, IntraEquipment equipment)
        {
            if (equipment == null)
                return;

            EquipmentId = equipment.Id;
            DamageNote = equipment.DamageNote;

            if (loan == null)
                return;
            
            LoanId = loan.Id;
            EmployeeId = loan.EmployeeId;
            DevolutionDate = loan.DevolutionDate;
        }

        public string LoanId { get; set; }
        public string EmployeeId { get; set; }
        public string EquipmentId { get; set; }
        public string DamageNote { get; set; }
        public DateTime DevolutionDate { get; set; }
    }
}
