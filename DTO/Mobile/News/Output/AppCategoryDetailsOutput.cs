using DTO.External.Wix.Database;
using DTO.General.Base.Api.Output;

namespace DTO.Mobile.News.Output
{
    public class AppCategoryDetailsOutput : BaseApiOutput
    {
        public AppCategoryDetailsOutput(string input) : base(input) { }

        public AppCategoryDetailsOutput(WixCategory input) : base(true) => Category = input;

        public WixCategory Category { get; set; }

    }
}
