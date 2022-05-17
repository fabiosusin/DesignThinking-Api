using DTO.General.Pagination.Input;

namespace DTO.General.Invoice.Input
{
    public class InvoiceCustomerListInput : PaginatorInput
    {
        public InvoiceCustomerListInput() { }
        public InvoiceCustomerListInput(int page, int result) => Paginator = new(page, result);
        public InvoiceCustomerListInput(InvoiceCustomerFiltersInput input) => Filters = input;
        public InvoiceCustomerListInput(InvoiceCustomerFiltersInput input, PaginatorInput paginator)
        {
            Filters = input;
            Paginator = paginator;
        }
        public InvoiceCustomerListInput(InvoiceCustomerFiltersInput input, int page, int result)
        {
            Filters = input;
            Paginator = new(page, result);
        }

        public InvoiceCustomerFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }

    }
}
