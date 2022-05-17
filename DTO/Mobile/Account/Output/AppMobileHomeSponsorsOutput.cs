using DTO.General.Base.Api.Output;
using DTO.General.Image.Input;
using DTO.Hub.Application.Sponsor.Database;
using System.Collections.Generic;

namespace DTO.Mobile.Account.Output
{
    public class AppMobileHomeSponsorsOutput : BaseApiOutput
    {
        public AppMobileHomeSponsorsOutput(string msg) : base(msg) { }
        public AppMobileHomeSponsorsOutput(IEnumerable<MobileHomeSponsorsData> sponsors) : base(true) => Sponsors = sponsors;

        public IEnumerable<MobileHomeSponsorsData> Sponsors { get; set; }
    }

    public class MobileHomeSponsorsData
    {
        public MobileHomeSponsorsData(AppSponsor input, ListResolutionsSize size)
        {
            if (input == null)
                return;

            Title = input.Title;
            Content = input.Content;
            ImageUrl = input.Image?.GetImage(size, FileType.Png);
        }

        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
    }
}