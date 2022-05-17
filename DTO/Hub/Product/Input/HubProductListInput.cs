using DTO.General.Pagination.Input;

namespace DTO.Hub.Product.Input
{
    public class HubProductListInput : PaginatorInput
    {
        public HubProductListInput() { }
        public HubProductListInput(int page, int result) => Paginator = new(page, result);
        public HubProductListInput(HubProductFiltersInput input, int page, int result)
        {
            Filters = input;
            Paginator = new(page, result);
        }

        public HubProductFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    }
}
