using DTO.General.Base.Database;
using static DTO.Hub.User.Enums.HubRouteRype;

namespace DTO.Hub.Permission.Database
{
    public class HubUserPermission : BaseData
    {
        public HubUserPermission(string allyId, string userId, string id, HubRouteType menus)
        {
            AllyId = allyId;
            UserId = userId;
            Menus = menus;
            Id = id;
        }
        public HubUserPermission(string allyId, string userId, HubRouteType menus)
        {
            AllyId = allyId;
            UserId = userId;
            Menus = menus;
        }
        public string AllyId { get; set; }
        public string UserId { get; set; }
        public HubRouteType Menus { get; set; }
    }
}
