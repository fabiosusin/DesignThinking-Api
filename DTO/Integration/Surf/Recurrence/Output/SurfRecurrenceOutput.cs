namespace DTO.Integration.Surf.Recurrence.Output
{
    public class SurfRecurrenceOutput
    {
        public string Message { get; set; }
        public SurfPayloadOutput Payload { get; set; }
    }

    public class SurfPayloadOutput
    {
        public string Id { get; set; }
    }
}
