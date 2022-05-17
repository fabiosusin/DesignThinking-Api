using DTO.Integration.SendPulse.Base.Output;
using Newtonsoft.Json;

namespace DTO.Integration.SendPulse.SMS.Output
{
    public class SendSmsResultOutput
    {
        public SendSmsResultOutput(string errorMessage) => Error = new(errorMessage);

        public SendSmsResultOutput(SendPulseResultOutput result)
        {
            if (result == null)
                return;

            Error = result.Errors;

            if (result.Json == null)
                return;

            var data = JsonConvert.DeserializeObject<SendSmsOutput>(result.Json);
            if (data != null)
                Customers = data;
        }

        public SendPulseApiError Error { get; set; }
        public SendSmsOutput Customers { get; set; }
    }
}
