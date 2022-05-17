using System.Collections.Generic;

namespace DTO.Hub.Customer.Input
{
    public class HubCustomerFiltersInput
    {
        public HubCustomerFiltersInput() { }
        public HubCustomerFiltersInput(string name)
        {
            Name = name;
        }

        public HubCustomerFiltersInput(IEnumerable<string> ids)
        {
            Ids = ids;
        }

        public IEnumerable<string> Ids { get; set; }
        public string AllyId { get; set; }
        public string Name { get; set; }
        public string Document { get; set; }
        public string Rg { get; set; }
    }
}
