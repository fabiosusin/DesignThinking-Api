namespace DTO.General.Surf.Input
{
    public class SurfDeaflympicsManagamentInput
    {
        public SurfDeaflympicsManagamentInput(string iccid, string surfPlanId, string ddd)
        {
            Iccid = iccid;
            SurfPlanId = surfPlanId;
            DDD = ddd;
        }

        public string Iccid { get; set; }
        public string SurfPlanId { get; set; }
        public string DDD { get; set; }
    }
}
