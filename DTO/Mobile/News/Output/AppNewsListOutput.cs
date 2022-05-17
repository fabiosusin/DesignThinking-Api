using DTO.General.Base.Api.Output;
using System.Collections.Generic;

namespace DTO.Mobile.News.Output
{
    public class AppNewsListOutput : BaseApiOutput
    {
        public AppNewsListOutput(string msg) : base(msg) { }

        public AppNewsListOutput(List<AppNewsOutput> news) : base(true) => News = news;

        public List<AppNewsOutput> News { get; set; }
    }
}
