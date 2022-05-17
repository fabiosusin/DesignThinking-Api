using DAO.DBConnection;
using DAO.Hub.Permission;
using DTO.General.Base.Api.Output;
using DTO.Hub.Permission.Database;

namespace Business.API.Hub.Permission
{
    public class BlAllyPermission
    {
        private readonly HubAllyPermissionDAO HubAllyPermissionDAO;
        public BlAllyPermission(XDataDatabaseSettings settings)
        {
            HubAllyPermissionDAO = new(settings);
        }

        public HubAllyPermission GetAllyPermission(string id) => HubAllyPermissionDAO.FindOne(x => x.AllyId == id);

        public BaseApiOutput UpsertAllyPermission(HubAllyPermission allyPermission) => HubAllyPermissionDAO.Upsert(allyPermission);
    }
}
