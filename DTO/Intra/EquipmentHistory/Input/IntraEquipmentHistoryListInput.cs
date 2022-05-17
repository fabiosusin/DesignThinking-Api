using DTO.General.Pagination.Input;

namespace DTO.Intra.EquipamentHistory.Input
{
    public class IntraEquipmentHistoryListInput : PaginatorInput
    {
        public IntraEquipmentHistoryListInput() { }
        public IntraEquipmentHistoryListInput(int page, int result) => Paginator = new(page, result);
        public IntraEquipmentHistoryListInput(IntraEquipmentHistoryFiltersInput input) => Filters = input;
        public IntraEquipmentHistoryListInput(IntraEquipmentHistoryFiltersInput input, int page, int result)
        {
            Filters = input;
            Paginator = new(page, result);
        }

        public IntraEquipmentHistoryFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    }
}
