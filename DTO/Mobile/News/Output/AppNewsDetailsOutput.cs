using DTO.External.Wix.Database;
using DTO.General.Base.Api.Output;

namespace DTO.Mobile.News.Output
{
    public class AppNewsDetailsOutput : BaseApiOutput
    {
        public AppNewsDetailsOutput(string input) : base(input) { }

        public AppNewsDetailsOutput(AppNewsOutput input) : base(true) => News = input;

        public AppNewsOutput News { get; set; }

    }
}
