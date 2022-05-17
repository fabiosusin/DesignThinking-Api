using DTO.General.Pagination.Input;

namespace DTO.Hub.Application.Sponsor.Input
{
    public class HubAppSponsorListInput : PaginatorInput
    {
        public HubAppSponsorListInput() { }
        public HubAppSponsorListInput(int page, int result) => Paginator = new(page, result);
        public HubAppSponsorListInput(HubAppSponsorFiltersInput input, int page, int result)
        {
            Filters = input;
            Paginator = new(page, result);
        }

        public HubAppSponsorFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }

    }
}