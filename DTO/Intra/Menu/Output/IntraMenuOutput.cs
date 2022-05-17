using DTO.Intra.Menu.Enum;
using DTO.Intra.User.Enums;
using System.Collections.Generic;

namespace DTO.Intra.Menu.Output
{
    public class IntraMenuOutput
    {
        public IntraMenuOutput(string name, IntraIconData iconData, string route)
        {
            Name = name;
            IconData = iconData;
            Route = route;
        }

        public IntraMenuOutput(string name, IntraIconData iconData, List<IntraMenuOutput> childrens)
        {
            Name = name;
            IconData = iconData;
            Children = childrens;
        }

        public string Name { get; set; }
        public string Route { get; set; }
        public IntraIconData IconData { get; set; }
        public List<IntraMenuOutput> Children { get; set; }
    }

    public class IntraIconData
    {
        public IntraIconData(string icon, IntraIconTypeEnum type)
        {
            Icon = icon;
            Type = type;
        }
        public string Icon { get; set; }
        public IntraIconTypeEnum Type { get; set; }
    }
}
