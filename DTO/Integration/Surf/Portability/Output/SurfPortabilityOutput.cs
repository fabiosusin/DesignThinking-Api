namespace DTO.Integration.Surf.Portability.Output
{
    public class SurfPortabilityOutput
    {
        public string Message { get; set; }
        public SurfPayloadPortabilityOutput Payload { get; set; }
    }

    public class SurfPayloadPortabilityOutput
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
    }
}
