using DTO.General.Pagination.Input;

namespace DTO.Mobile.Visao.Input
{
    public class AppCameraListInput
    {
        public AppCameraListInput(AppFiltersCameraInput filters, PaginatorInput paginator)
        {
            Filters = filters;
            Paginator = paginator;
        }

        public AppFiltersCameraInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    }
}
