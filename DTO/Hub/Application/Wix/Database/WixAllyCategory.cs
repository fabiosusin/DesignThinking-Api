using DTO.General.Base.Database;

namespace DTO.Hub.Application.Wix.Database
{
    public class WixAllyCategory : BaseData
    {
        public WixAllyCategory() { }
        public WixAllyCategory(string allyId, string categoryId)
        {
            AllyId = allyId;
            CategoryId = categoryId;
        }

        public string AllyId { get; set; }
        public string CategoryId { get; set; }
    }
}
