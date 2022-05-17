using DTO.General.Pagination.Input;

namespace DTO.Hub.Customer.Input
{
    public class HubCustomerListInput : PaginatorInput
    {
        public HubCustomerListInput() { }
        public HubCustomerListInput(int page, int result) => Paginator = new(page, result);
        public HubCustomerListInput(HubCustomerFiltersInput input) => Filters = input;
        public HubCustomerListInput(HubCustomerFiltersInput input, int page, int result)
        {
            Filters = input;
            Paginator = new(page, result);
        }

        public HubCustomerFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    
    }
}
