using DTO.Hub.Order.Enum;
using System;
using System.Collections.Generic;

namespace DTO.Hub.Order.Input
{
    public class HubOrderFiltersInput
    {
        public HubOrderFiltersInput() { }
        public HubOrderFiltersInput(IEnumerable<string> ids) => Ids = ids;
        public IEnumerable<string> Ids { get; set; }
        public long? Code { get; set; }
        public string AllyId { get; set; }
        public string SellerName { get; set; }
        public string CustomerName { get; set; }
        public string AccountPlanName { get; set; }
        public HubOrderStatusEnum Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
