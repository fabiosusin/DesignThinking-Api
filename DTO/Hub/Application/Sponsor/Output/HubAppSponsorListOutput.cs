using DTO.General.Base.Api.Output;
using DTO.Hub.Application.Sponsor.Database;
using System.Collections.Generic;

namespace DTO.Hub.Application.Sponsor.Output
{
    public class HubAppSponsorListOutput : BaseApiOutput
    {
        public HubAppSponsorListOutput(string msg) : base(msg) { }
        public HubAppSponsorListOutput(IEnumerable<AppSponsorListData> sponsors) : base(true) => Sponsors = sponsors;
        public IEnumerable<AppSponsorListData> Sponsors { get; set; }
    }

    public class AppSponsorListData
    {
        public AppSponsorListData(AppSponsor input, string img)
        {
            if (input == null)
                return;

            Id = input.Id;
            Title = input.Title;
            Content = input.Content;
            AllyId = input.AllyId;
            ImageUrl = img;
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string AllyId { get; set; }
        public bool Linked { get; set; }
    }
}
