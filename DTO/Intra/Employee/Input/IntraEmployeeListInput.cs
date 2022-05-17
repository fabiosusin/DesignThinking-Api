using DTO.General.Pagination.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Intra.Employee.Input
{
    public class IntraEmployeeListInput : PaginatorInput
    {
        public IntraEmployeeFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    }
}
