using DTO.General.Base.Database;
using DTO.General.Image.Input;
using DTO.Hub.Application.Sponsor.Input;

namespace DTO.Hub.Application.Sponsor.Database
{
    public class AppSponsor : BaseData
    {
        public AppSponsor() { }
        public AppSponsor(HubAppSponsorInput input, ImageFormat img) {
            if (input == null)
                return;

            Title = input.Title;
            Content = input.Content;
            AllyId = input.AllyId;
            Image = img;
        }

        public AppSponsor(string id, HubAppSponsorInput input, ImageFormat img)
        {
            if (input == null)
                return;

            Id = id;
            Title = input.Title;
            Content = input.Content;
            AllyId = input.AllyId;
            Image = img;
        }

        public string Title { get; set; }
        public string Content { get; set; }
        public string AllyId { get; set; }
        public ImageFormat Image { get; set; }
    }
}
