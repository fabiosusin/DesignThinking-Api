using DTO.General.Pagination.Input;

namespace DTO.Mobile.News.Input
{

    public class AppNewsListInput
    {
        public AppNewsListInput(AppNewsFiltersInput filters, PaginatorInput paginator)
        {
            Filters = filters;
            Paginator = paginator;
        }

        public AppNewsFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    }
}
