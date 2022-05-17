using DTO.General.Pagination.Input;

namespace DTO.Hub.Order.Input
{
    public class HubOrderListInput : PaginatorInput
    {
        public HubOrderListInput() { }
        public HubOrderListInput(int page, int result) => Paginator = new(page, result);
        public HubOrderListInput(HubOrderFiltersInput input) => Filters = input;
        public HubOrderListInput(HubOrderFiltersInput input, int page, int result)
        {
            Filters = input;
            Paginator = new(page, result);
        }
        public HubOrderFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    }
}
