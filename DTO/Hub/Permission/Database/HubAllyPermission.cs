using DTO.General.Base.Database;
using System.Collections.Generic;

namespace DTO.Hub.Permission.Database
{
    public class HubAllyPermission : BaseData
    {
        public string AllyId { get; set; }
        public List<HubContentPermission> ProductCategories { get; set; }
    }

    public class HubContentPermission
    {
        public string DataId { get; set; }
        public bool Valid { get; set; }
    }
}
