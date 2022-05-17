namespace DTO.Hub.Application.Faq.Input
{
    public class HubAppFaqInput
    {
        public string Id { get; set; }
        public string AllyId { get; set; }
        public string UserId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool Linked { get; set; }
    }
}
