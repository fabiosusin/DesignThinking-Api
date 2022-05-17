using System;

namespace DTO.Hub.User.Enums
{
    public class HubRouteRype
    {
        [Flags]
        public enum HubRouteType
        {
            Unknown = 0,
            Orders = 1 << 0,
            Cellphones = 1 << 1,
            Users = 1 << 2,
            Customers = 1 << 3,
            Products = 1 << 4,
            Ally = 1 << 5,
            AccountPlans = 1 << 6,
            OrdersReport = 1 << 7
        }
    }
}
