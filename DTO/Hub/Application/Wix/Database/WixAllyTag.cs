using DTO.General.Base.Database;

namespace DTO.Hub.Application.Wix.Database
{
    public class WixAllyTag : BaseData
    {
        public WixAllyTag() { }
        public WixAllyTag(string allyId, string tagId)
        {
            AllyId = allyId;
            TagId = tagId;
        }

        public string AllyId { get; set; }
        public string TagId { get; set; }
    }
}
