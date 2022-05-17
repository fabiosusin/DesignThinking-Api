using DTO.General.Pagination.Input;

namespace DTO.Hub.Ally.Input
{
    public class HubAllyListInput : PaginatorInput
    {
        public HubAllyListInput() { }
        public HubAllyListInput(int page, int result) => Paginator = new(page, result);
        public HubAllyListInput(HubAllyFiltersInput input, int page, int result)
        {
            Filters = input;
            Paginator = new(page, result);
        }

        public HubAllyFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    }
}
