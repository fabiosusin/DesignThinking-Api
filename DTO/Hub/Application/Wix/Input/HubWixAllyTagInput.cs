namespace DTO.Hub.Application.Wix.Input
{
    public class HubWixAllyTagInput
    {
        public HubWixAllyTagInput() { }
        public HubWixAllyTagInput(string allyId, string tagId)
        {
            AllyId = allyId;
            TagId = tagId;
        }
        public string AllyId { get; set; }
        public string TagId { get; set; }
        public bool Linked { get; set; }
    }
}
