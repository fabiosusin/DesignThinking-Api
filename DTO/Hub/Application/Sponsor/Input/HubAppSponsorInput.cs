namespace DTO.Hub.Application.Sponsor.Input
{
    public class HubAppSponsorInput
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AllyId { get; set; }
        public string ImageBase64 { get; set; }
        public bool SaveImg { get; set; }
    }
}
