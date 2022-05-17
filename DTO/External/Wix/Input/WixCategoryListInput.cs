using DTO.General.Pagination.Input;

namespace DTO.External.Wix.Input
{
    public class WixCategoryListInput : PaginatorInput
    {
        public WixCategoryListInput() { }
        public WixCategoryListInput(int page, int result) => Paginator = new(page, result);
        public WixCategoryListInput(WixCategoryFiltersInput input, int page, int result)
        {
            Filters = input;
            Paginator = new(page, result);
        }

        public WixCategoryFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }

    }
}
