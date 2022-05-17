using DTO.Integration.Wix.Base;

namespace DTO.Integration.Wix.Category.Output
{

    public class WixCategoryDataOutput
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public int PostCount { get; set; }
        public string Title { get; set; }
        public WixDataCoverMedia CoverMedia { get; set; }
        public int Rank { get; set; }
        public string Language { get; set; }
        public string Slug { get; set; }
    }
}
