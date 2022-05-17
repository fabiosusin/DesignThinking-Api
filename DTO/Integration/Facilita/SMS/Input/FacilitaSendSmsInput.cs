using DTO.Integration.SendPulse.SMS.Input;
using System.Linq;

namespace DTO.Integration.Facilita.SMS.Input
{
    public class FacilitaSendSmsInput
    {
        public FacilitaSendSmsInput(SendSmsInput input)
        {
            if (input == null)
                return;

            Destinatario = input.Phones?.FirstOrDefault();
            Msg = input.Body;
        }

        public string Destinatario { get; set; }
        public string Msg { get; set; }
    }
}
