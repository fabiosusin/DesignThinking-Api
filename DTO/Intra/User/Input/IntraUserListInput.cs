using DTO.General.Pagination.Input;

namespace DTO.Intra.User.Input
{
    public class IntraUserListInput : PaginatorInput
    {
        public IntraUserListInput() { }
        public IntraUserListInput(int page, int result) => Paginator = new(page, result);
        public IntraUserListInput(IntraUserFiltersInput input, int page, int result)
        {
            Filters = input;
            Paginator = new(page, result);
        }

        public IntraUserFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    }
}
