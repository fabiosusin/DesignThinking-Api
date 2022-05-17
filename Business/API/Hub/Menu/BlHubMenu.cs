using DAO.DBConnection;
using DAO.Hub.Permission;
using DTO.Hub.Menu.Output;
using DTO.Hub.Permission.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using static DTO.Hub.User.Enums.HubRouteRype;

namespace Business.API.Hub.Menu
{
    public class BlHubMenu
    {
        private readonly HubMenuDAO HubMenuDAO;
        private readonly HubUserPermissionDAO HubUserPermissionDAO;

        public BlHubMenu(XDataDatabaseSettings settings)
        {
            HubMenuDAO = new(settings);
            HubUserPermissionDAO = new(settings);
        }

        public List<HubMenuOutput> GetHubMenu(string allyId, string userId)
        {
            var userMenu = HubUserPermissionDAO.FindOne(x => x.AllyId == allyId && x.UserId == userId);
            if (userMenu == null)
                return null;

            var menus = HubMenuDAO.GetMenusByHubRouteType(userMenu.Menus);
            var result = new List<HubMenuOutput>();

            foreach (var item in menus)
                result.Add(new HubMenuOutput(item.Name, item.Route, item.IconData, item.Children?.Select(x => new HubMenuOutput(x.Type, x.Name, x.Route, x.HasPermission, x.IconData)).ToList()));

            return result;
        }

        public List<HubMenuOutput> GetHubMenuPermissionOptions(string allyId, string userId)
        {
            var result = new List<HubMenuOutput>();
            var menus = HubMenuDAO.FindAll();
            var userMenu = HubUserPermissionDAO.FindOne(x => x.AllyId == allyId && x.UserId == userId);

            var permissions = HubMenuDAO.GetMenusByHubRouteType(userMenu?.Menus ?? HubRouteType.Unknown);
            foreach (var menu in menus)
            {
                var currentMenu = permissions?.FirstOrDefault(x => x.Name == menu.Name);
                if (!(menu.Children?.Any() ?? false) && currentMenu == null)
                    continue;

                foreach (var child in menu.Children?.Where(y => currentMenu?.Children?.FirstOrDefault(x => x.Name == y.Name) != null).ToList() ?? new List<HubChildMenu>())
                    child.HasPermission = true;

                result.Add(new(menu.Name, menu.Route, menu.IconData, menu.Children?.Select(x => new HubMenuOutput(x.Type, x.Name, x.Route, x.HasPermission, x.IconData)).ToList()));
            }

            return result;
        }
    }
}
