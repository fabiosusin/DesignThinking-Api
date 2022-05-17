using DTO.General.Image.Input;

namespace DTO.Hub.Application.Sponsor.Input
{
    public class HubAppSponsorFiltersInput
    {
        public string AllyId { get; set; }
        public string Title { get; set; }
        public ListResolutionsSize ImageSize { get; set; }
    }
}
