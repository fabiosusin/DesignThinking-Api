using DTO.Integration.Wix.Base;
using System.Collections.Generic;

namespace DTO.Integration.Wix.Category.Output
{
    public class GetWixCategoryListOutput
    {
        public List<WixCategoryDataOutput> Categories { get; set; }
        public WixMetaData MetaData { get; set; }
    }
}
