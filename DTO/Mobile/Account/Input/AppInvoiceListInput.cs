using DTO.General.Pagination.Input;

namespace DTO.Mobile.Account.Input
{
    public class AppInvoiceListInput
    {
        public AppInvoiceFilters Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    }

    public class AppInvoiceFilters
    {
        public string MobileId { get; set; }
        public string Number { get; set; }
        public string AllyId { get; set; }
    }
}
