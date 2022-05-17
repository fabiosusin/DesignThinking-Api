using DTO.External.Wix.Database;
using DTO.General.Base.Api.Output;
using System.Collections.Generic;

namespace DTO.External.Wix.Output
{
    public class WixCategoryListOutput : BaseApiOutput
    {
        public WixCategoryListOutput(string msg) : base(msg) { }
        public WixCategoryListOutput(IEnumerable<WixCategoryListData> categories) : base(true) => Categories = categories;
        public IEnumerable<WixCategoryListData> Categories { get; set; }
    }

    public class WixCategoryListData
    {
        public WixCategoryListData(WixCategory input, bool linked)
        {
            if (input == null)
                return;

            Id = input.Id;
            Title = input.Title;
            WixCategoryId = input.WixCategoryId;
            PostCount = input.PostCount;
            Rank = input.Rank;
            Linked = linked;
        }
        public string Id { get; set; }
        public string Title { get; set; }
        public string WixCategoryId { get; set; }
        public int PostCount { get; set; }
        public int Rank { get; set; }
        public bool Linked { get; set; }
    }
}
