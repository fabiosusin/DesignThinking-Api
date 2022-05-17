using DTO.General.Pagination.Input;

namespace DTO.Hub.Cellphone.Input
{
    public class HubRecurrenceListInput
    {
        public HubRecurrenceListInput() { }
        public HubRecurrenceListInput(int page, int result) => Paginator = new(page, result);
        public HubRecurrenceListInput(HubRecurrenceFiltersInput input) => Filters = input;
        public HubRecurrenceListInput(HubRecurrenceFiltersInput input, int page, int result)
        {
            Filters = input;
            Paginator = new(page, result);
        }

        public HubRecurrenceFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    }
}
