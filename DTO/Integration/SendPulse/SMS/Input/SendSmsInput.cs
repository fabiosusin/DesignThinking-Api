using System.Collections.Generic;

namespace DTO.Integration.SendPulse.SMS.Input
{
    public class SendSmsInput
    {
        public SendSmsInput(string sender, string body, string number)
        {
            Sender = sender;
            Body = body;
            Phones = new List<string> { number };
        }

        public string Sender { get; set; }
        public string Body { get; set; }
        public List<string> Phones { get; set; }
    }
}
