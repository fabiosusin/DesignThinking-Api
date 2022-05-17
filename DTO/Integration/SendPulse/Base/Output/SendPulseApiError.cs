using Newtonsoft.Json;

namespace DTO.Integration.SendPulse.Base.Output
{
    public class SendPulseApiError
    {
        public SendPulseApiError() { }
        public SendPulseApiError(string message) => Message = message;

        public string Message { get; set; }
        [JsonProperty("error_code")]
        public int ErrorCode { get; set; }
    }

}
