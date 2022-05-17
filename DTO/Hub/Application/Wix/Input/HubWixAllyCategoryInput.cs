namespace DTO.Hub.Application.Wix.Input
{
    public class HubWixAllyCategoryInput
    {
        public HubWixAllyCategoryInput() { }
        public HubWixAllyCategoryInput(string allyId, string categoryId)
        {
            AllyId = allyId;
            CategoryId = categoryId;
        }
        public string AllyId { get; set; }
        public string CategoryId { get; set; }
        public bool Linked { get; set; }
    }
}
