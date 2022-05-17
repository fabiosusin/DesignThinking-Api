namespace DTO.Integration.Surf.Portability.Input
{
    public class SurfCheckPortabilityStatusInput
    {
        public SurfCheckPortabilityStatusInput(string currentMsisdn, string portabilityMsisdn, string document)
        {
            CurrentMsisdn = currentMsisdn;
            PortabilityMsisdn = portabilityMsisdn;
            Document = document;
        }

        public string CurrentMsisdn { get; set; }
        public string PortabilityMsisdn { get; set; }
        public string Document { get; set; }
    }
}
