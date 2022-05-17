using DTO.General.Pagination.Input;

namespace DTO.Intra.Loan.Input
{
    public class IntraLoanListInput
    {
        public IntraLoanListInput() { }
        public IntraLoanListInput(int page, int result) => Paginator = new(page, result);
        public IntraLoanListInput(IntraLoanFiltersInput input, int page, int result)
        {
            Filters = input;
            Paginator = new(page, result);
        }

        public IntraLoanFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    }
}
