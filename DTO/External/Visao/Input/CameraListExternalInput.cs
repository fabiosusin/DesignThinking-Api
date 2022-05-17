using DTO.General.Pagination.Input;

namespace DTO.External.Visao.Input
{
    public class CameraListExternalInput
    {
        public CameraListExternalInput(FiltersCameraExternalInput filters, PaginatorInput paginator)
        {
            Filters = filters;
            Paginator = paginator;
        }

        public FiltersCameraExternalInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    }
}
