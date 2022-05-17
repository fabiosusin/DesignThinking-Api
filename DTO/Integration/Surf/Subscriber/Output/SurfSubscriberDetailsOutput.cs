using DTO.Integration.Surf.BaseDetails.Output;

namespace DTO.Integration.Surf.Subscriber.Output
{
    public class SurfSubscriberDetailsOutput : SurfDetailsBaseApiOutput
    {
        public string Description { get; set; }
        public string CPF { get; set; }
    }
}
