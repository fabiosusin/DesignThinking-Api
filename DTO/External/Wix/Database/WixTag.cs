using DTO.External.Wix.Input;
using DTO.General.Base.Database;

namespace DTO.External.Wix.Database
{
    public class WixTag : BaseData
    {
        public WixTag() { }
        public WixTag(WixTagRegisterInput input)
        {
            if (input == null)
                return;

            WixTagId = input.Id;
            Title = input.Label;
            Slug = input.Slug;
        }

        public string WixTagId { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
    }
}
