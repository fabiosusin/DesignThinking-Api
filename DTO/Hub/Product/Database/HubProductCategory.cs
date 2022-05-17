using DTO.General.Base.Database;
using DTO.Hub.Customer.Enum;
using DTO.Hub.Menu.Enum;
using DTO.Hub.Product.Enum;

namespace DTO.Hub.Product.Database
{
    public class HubProductCategory : BaseData
    {
        public HubProductCategory(string name)
        {
            Name = name;
            Active = true;
            PersonType = HubDocumentTypeEnum.Both;
            DiscountEnabled = true;
        }

        public HubProductCategory(string name, HubProductTypeEnum type, HubIconData iconData)
        {
            Name = name;
            Active = true;
            PersonType = HubDocumentTypeEnum.Both;
            CategoryType = type;
            DiscountEnabled = true;
            IconData = iconData;
        }

        public string Name { get; set; }
        public bool DiscountEnabled { get; set; }
        public bool Active { get; set; }
        public HubIconData IconData { get; set; }
        public HubDocumentTypeEnum PersonType { get; set; }
        public HubProductTypeEnum CategoryType { get; set; }
    }

    public class HubIconData
    {
        public HubIconData() { }
        public HubIconData(string icon, HubIconTypeEnum type)
        {
            Icon = icon;
            Type = type;
        }

        public string Icon { get; set; }
        public HubIconTypeEnum Type { get; set; }
    }
}
