using DTO.General.Base.Api.Output;
using DTO.Hub.Customer.Database;
using System.Collections.Generic;

namespace DTO.Hub.Customer.Output
{
    public class HubCustomerListOutput : BaseApiOutput
    {
        public HubCustomerListOutput(string msg) : base(msg) { }
        public HubCustomerListOutput(IEnumerable<HubCustomer> allys) : base(true) => Customers = allys;
        public IEnumerable<HubCustomer> Customers { get; set; }
    }
}
