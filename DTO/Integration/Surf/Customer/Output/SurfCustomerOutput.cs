using System.Collections.Generic;

namespace DTO.Integration.Surf.Customer.Output
{
    public class SurfCustomerOutput
    {
        public string Message { get; set; }
        public SurfCustomerPayloadOutput Payload { get; set; }
    }

    public class SurfCustomerPayloadOutput
    {
        public string CustomerId { get; set; }
        public string Document { get; set; }
        public string Name { get; set; }
        public int Ddd { get; set; }
        public List<SurfCustomerSubscriptionOutput> Subscriptions { get; set; }
    }

    public class SurfCustomerPlanOutput
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string DurationTime { get; set; }
    }

    public class SurfCustomerSubscriptionOutput
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Document { get; set; }
        public int Ddd { get; set; }
        public string Msisdn { get; set; }
        public string Iccid { get; set; }
        public SurfCustomerPlanOutput Plan { get; set; }
    }
}
