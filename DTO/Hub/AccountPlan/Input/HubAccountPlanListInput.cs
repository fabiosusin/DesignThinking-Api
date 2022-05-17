using DTO.General.Pagination.Input;

namespace DTO.Hub.AccountPlan.Input
{
    public class HubAccountPlanListInput : PaginatorInput
    {
        public HubAccountPlanListInput() { }
        public HubAccountPlanListInput(int page, int result) => Paginator = new(page, result);
        public HubAccountPlanListInput(string name, int page, int result)
        {
            Filters = new(name);
            Paginator = new(page, result);
        }

        public HubAccountPlanFiltersInput Filters { get; set; }
        public PaginatorInput Paginator { get; set; }
    }
}
