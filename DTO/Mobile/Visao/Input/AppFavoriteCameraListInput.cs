using DTO.General.Pagination.Input;

namespace DTO.Mobile.Visao.Input
{
    public class AppFavoriteCameraListInput
    {
        public AppFavoriteCameraListInput(AppFiltersFavoriteCameraInput filters, PaginatorInput paginator)
        {
            Filters = filters;
            Paginator = paginator;
        }

        public AppFiltersFavoriteCameraInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    }
}
