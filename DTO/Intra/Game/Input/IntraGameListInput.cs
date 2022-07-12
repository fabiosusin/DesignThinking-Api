using DTO.General.Pagination.Input;

namespace DTO.Intra.Game.Input
{
    public class IntraGameListInput : PaginatorInput
    {
        public IntraGameFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    }
}
