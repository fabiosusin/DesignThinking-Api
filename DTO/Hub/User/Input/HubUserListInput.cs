using DTO.General.Pagination.Input;

namespace DTO.Hub.User.Input
{
    public class HubUserListInput : PaginatorInput
    {
        public HubUserListInput() { }
        public HubUserListInput(int page, int result) => Paginator = new(page, result);
        public HubUserListInput(HubUserFiltersInput input, int page, int result)
        {
            Filters = input;
            Paginator = new(page, result);
        }

        public HubUserFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    }
}
