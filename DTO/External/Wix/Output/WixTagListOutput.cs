using DTO.External.Wix.Database;
using DTO.General.Base.Api.Output;
using System.Collections.Generic;

namespace DTO.External.Wix.Output
{
    public class WixTagListOutput : BaseApiOutput
    {
        public WixTagListOutput(string msg) : base(msg) { }
        public WixTagListOutput(IEnumerable<WixTagListData> tags) : base(true) => Tags = tags;
        public IEnumerable<WixTagListData> Tags { get; set; }
    }

    public class WixTagListData
    {
        public WixTagListData(WixTag input, bool linked)
        {
            if (input == null)
                return;

            Id = input.Id;
            Title = input.Title;
            WixTagId = input.WixTagId;
            Linked = linked;
        }
        public string Id { get; set; }
        public string Title { get; set; }
        public string WixTagId { get; set; }
        public bool Linked { get; set; }
    }
}
