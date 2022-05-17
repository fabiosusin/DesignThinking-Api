using DTO.General.Pagination.Input;

namespace DTO.Intra.Equipament.Input
{
    public class IntraEquipmentListInput : PaginatorInput
    {
        public IntraEquipmentListInput() { }
        public IntraEquipmentListInput(int page, int result) => Paginator = new(page, result);
        public IntraEquipmentListInput(IntraEquipmentFiltersInput input) => Filters = input;
        public IntraEquipmentListInput(IntraEquipmentFiltersInput input, int page, int result)
        {
            Filters = input;
            Paginator = new(page, result);
        }

        public IntraEquipmentFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    }
}
