using DTO.Integration.Surf.Token.Input;

namespace DTO.Integration.Surf.Subscriber.Input
{
    public class SurfSubscriberDetailsInput : SurfDetailsBaseApiInput
    {
        public SurfSubscriberDetailsInput() { }
        public SurfSubscriberDetailsInput(string msisdn) : base(msisdn) { }
    }
}
