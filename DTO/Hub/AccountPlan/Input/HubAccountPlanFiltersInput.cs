namespace DTO.Hub.AccountPlan.Input
{
    public class HubAccountPlanFiltersInput
    {
        public HubAccountPlanFiltersInput(string name) => Name = name;

        public string Name { get; set; }
    }
}
