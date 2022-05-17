namespace DTO.Integration.Surf.Portability.Output
{
    public class SurfResendPortabilitySmsOutput
    {
        public string Message { get; set; }
        public PortabilitySmsResult Payload { get; set; }
    }

    public class PortabilitySmsResult
    {
        public string Msisdn { get; set; }
        public string Document { get; set; }
    }
}
