using DAO.Base;
using DAO.DBConnection;
using DTO.General.DAO.Output;
using DTO.Hub.Permission.Database;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using static DTO.Hub.User.Enums.HubRouteRype;

namespace DAO.Hub.Permission
{
    public class HubMenuDAO : IBaseDAO<HubMenu>
    {
        internal RepositoryMongo<HubMenu> Repository;
        public HubMenuDAO(IXDataDatabaseSettings settings) => Repository = new(settings?.MongoDBSettings);

        public DAOActionResultOutput Insert(HubMenu obj)
        {
            var result = Repository.Insert(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Update(HubMenu obj)
        {
            var result = Repository.Update(obj);
            if (string.IsNullOrEmpty(result?.Id))
                return new("Não foi possível salvar o registro");

            return new(result);
        }

        public DAOActionResultOutput Upsert(HubMenu obj) => string.IsNullOrEmpty(obj.Id) ? Insert(obj) : Update(obj);

        public DAOActionResultOutput Remove(HubMenu obj)
        {
            Repository.RemoveById(obj.Id);
            return new(true);
        }

        public DAOActionResultOutput RemoveById(string id)
        {
            Repository.RemoveById(id);
            return new(true);
        }

        public HubMenu FindOne() => Repository.FindOne();

        public HubMenu FindOne(Expression<Func<HubMenu, bool>> predicate) => Repository.FindOne(predicate);

        public HubMenu FindById(string id) => Repository.FindById(id);

        public IEnumerable<HubMenu> Find(Expression<Func<HubMenu, bool>> predicate) => Repository.Collection.Find(Query<HubMenu>.Where(predicate));

        public IEnumerable<HubMenu> FindAll() => Repository.FindAll();

        public List<HubMenu> GetMenusByHubRouteType(HubRouteType type)
        {
            var result = new List<HubMenu>();
            var menus = FindAll();
            foreach (var menu in menus)
            {
                if (!(menu.Children?.Any() ?? false))
                {
                    result.Add(menu);
                    continue;
                }

                var children = menu.Children?.Where(x => type.HasFlag(x.Type));
                if (!(children?.Any() ?? false))
                    continue;


                var menuData = menu;
                menuData.Children = children.ToList();
                result.Add(menuData);
            }
            return result;
        }
    }
}
