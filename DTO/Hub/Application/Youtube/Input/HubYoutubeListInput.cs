using DTO.General.Pagination.Input;

namespace DTO.Hub.Application.Youtube.Input
{
    public class HubYoutubeListInput : PaginatorInput
    {
        public HubYoutubeListInput() { }
        public HubYoutubeListInput(int page, int result) => Paginator = new(page, result);
        public HubYoutubeListInput(HubYoutubeFiltersInput input, int page, int result)
        {
            Filters = input;
            Paginator = new(page, result);
        }
        public HubYoutubeListInput(HubYoutubeFiltersInput input)
        {
            Filters = input;
        }

        public HubYoutubeFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    
    }
}
