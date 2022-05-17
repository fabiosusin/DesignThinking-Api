using DTO.Hub.Menu.Enum;
using System.Collections.Generic;
using static DTO.Hub.User.Enums.HubRouteRype;

namespace DTO.Hub.Menu.Output
{
    public class HubMenuOutput
    {
        public HubMenuOutput(HubRouteType type, string name, string route, bool hasPermission, HubIconData iconData)
        {
            Type = type;
            Name = name;
            Route = route;
            HasPermission = hasPermission;
            IconData = iconData;
        }

        public HubMenuOutput(string name, string route, HubIconData iconData, List<HubMenuOutput> childrens)
        {
            Name = name;
            Route = route;
            IconData = iconData;
            Children = childrens;
        }
        public bool HasPermission { get; set; }
        public string Name { get; set; }
        public string Route { get; set; }
        public HubIconData IconData { get; set; }
        public HubRouteType Type { get; set; }
        public List<HubMenuOutput> Children { get; set; }
    }

    public class HubIconData
    {
        public HubIconData(string icon, HubIconTypeEnum type)
        {
            Icon = icon;
            Type = type;
        }
        public string Icon { get; set; }
        public HubIconTypeEnum Type { get; set; }
    }
}
