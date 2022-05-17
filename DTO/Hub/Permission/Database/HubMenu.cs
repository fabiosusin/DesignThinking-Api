using DTO.General.Base.Database;
using DTO.Hub.Menu.Output;
using System.Collections.Generic;
using static DTO.Hub.User.Enums.HubRouteRype;

namespace DTO.Hub.Permission.Database
{
    public class HubMenu : BaseData
    {
        public string Name { get; set; }
        public string Route { get; set; }
        public HubIconData IconData { get; set; }
        public List<HubChildMenu> Children { get; set; }
    }

    public class HubChildMenu
    {
        public HubChildMenu()
        {
            HasPermission = false;
        }
        public HubRouteType Type { get; set; }
        public bool HasPermission { get; set; }
        public string ChildId { get; set; }
        public string Name { get; set; }
        public string Route { get; set; }
        public HubIconData IconData { get; set; }
        public int FlagValue { get; set; }
    }
}
