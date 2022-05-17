namespace DTO.Integration.Sige.AccountPlan.Input
{
    public class SigeAccountPlanFiltersInput
    {
        public SigeAccountPlanFiltersInput(int pageSize, int skip)
        {
            PageSize = pageSize;    
            Skip = skip;
        }

        public string Name { get; set; }
        public string TipoLancamento { get; set; }
        public bool SomentePrimeiroNivel { get; set; }
        public int PageSize { get; set; }
        public int Skip { get; set; }
    }
}
