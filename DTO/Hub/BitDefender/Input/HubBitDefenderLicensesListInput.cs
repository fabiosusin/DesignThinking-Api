using DTO.General.Pagination.Input;

namespace DTO.Hub.BitDefender.Input
{
    public class HubBitDefenderLicensesListInput
    {
        public HubBitDefenderLicensesListInput() { }
        public HubBitDefenderLicensesListInput(int page, int result) => Paginator = new(page, result);
        public HubBitDefenderLicensesListInput(HubBitDefenderLicensesFiltersInput input) => Filters = input;
        public HubBitDefenderLicensesListInput(HubBitDefenderLicensesFiltersInput input, int page, int result)
        {
            Filters = input;
            Paginator = new(page, result);
        }

        public HubBitDefenderLicensesFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    }
}
