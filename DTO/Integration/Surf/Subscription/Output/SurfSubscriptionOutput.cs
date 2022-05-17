using System.Collections.Generic;

namespace DTO.Integration.Surf.Subscription.Output
{
    public class SurfSubscriptionOutput
    {
        public string Message { get; set; }
        public List<SurfSubscriptionContentOutput> Payload { get; set; }
    }

    public class SurfSubscriptionContentOutput
    {
        public string Id { get; set; }
        public bool Success { get; set; }
        public string LeftLabel { get; set; }
        public string RightLabel { get; set; }
        public string Description { get; set; }
        public string Iccid { get; set; }
        public string Msisdn { get; set; }

        public string PortabilityStatus { get; set; }
        public string PortabilityDescription { get; set; }
        public string Plan { get; set; }
    }
}
