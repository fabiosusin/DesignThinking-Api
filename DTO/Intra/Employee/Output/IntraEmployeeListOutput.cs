using DTO.General.Base.Api.Output;
using DTO.Intra.Employee.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Intra.Employee.Output
{
    public class IntraEmployeeListOutput : BaseApiOutput
    {
        public IntraEmployeeListOutput(string msg) : base(msg) { }
        public IntraEmployeeListOutput(IEnumerable<IntraEmployee> allys) : base(true) => Employees = allys;
        public IEnumerable<IntraEmployee> Employees { get; set; }
    }
}
