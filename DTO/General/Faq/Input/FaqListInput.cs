using DTO.General.Pagination.Input;

namespace DTO.General.Faq.Input
{
    public class FaqListInput : PaginatorInput
    {
        public FaqListInput() { }
        public FaqListInput(int page, int result) => Paginator = new(page, result);
        public FaqListInput(FaqFiltersInput input, int page, int result)
        {
            Filters = input;
            Paginator = new(page, result);
        }
        public FaqFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    }
}
