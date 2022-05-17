namespace DTO.Integration.SendPulse.Base.Output
{
    public class SendPulseResultOutput
    {
        public SendPulseResultOutput(SendPulseApiError errors) => Errors = errors;
        public SendPulseResultOutput(string json) => Json = json;

        public SendPulseApiError Errors { get; set; }
        public string Json { get; set; }
    }
}
