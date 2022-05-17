using DTO.General.Pagination.Input;

namespace DTO.External.Wix.Input
{
    public class WixTagListInput : PaginatorInput
    {
        public WixTagListInput() { }
        public WixTagListInput(int page, int result) => Paginator = new(page, result);
        public WixTagListInput(WixTagFiltersInput input, int page, int result)
        {
            Filters = input;
            Paginator = new(page, result);
        }

        public WixTagFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }

    }
}
