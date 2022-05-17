using DTO.General.Base.Api.Output;
using DTO.General.Base.Output;
using DTO.Intra.Equipament.Database;
using DTO.Intra.Loan.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DTO.Intra.Loan.Output
{
    public class IntraLoanDetailsOutput : BaseApiOutput
    {
        public IntraLoanDetailsOutput(string message) : base(message) { }
        public IntraLoanDetailsOutput(IntraLoanDetails details) : base(true) => Details = details;
        public IntraLoanDetails Details { get; set; }
    }

    public class IntraLoanDetails
    {
        public IntraLoanDetails() { }
        public IntraLoanDetails(IntraLoan loan, IEnumerable<IntraEquipment> equipments, string employeeName)
        {
            EmployeeName = employeeName;

            if (loan == null)
                return;

            Id = loan.Id;
            LoanDate = loan.LoanDate;
            DevolutionDate = loan.DevolutionDate;

            if (!(equipments?.Any() ?? false))
                return;

            Equipments = equipments.Select(x => new IntraEquipmentDetails(x.Id, $"{x.Name} - {x.Code}", x.DamageNote)).ToList();
        }

        public string Id { get; set; }
        public string EmployeeName { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DevolutionDate { get; set; }
        public List<IntraEquipmentDetails> Equipments { get; set; }
    }

    public class IntraEquipmentDetails : BaseInfoOutput
    {
        public IntraEquipmentDetails() { }
        public IntraEquipmentDetails(string id, string name, string note) : base(id, name) => DamageNote = note;
        public string DamageNote { get; set; }
    }
}
