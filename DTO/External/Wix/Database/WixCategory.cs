using DTO.General.Base.Database;
using DTO.Integration.Wix.Category.Output;

namespace DTO.External.Wix.Database
{
    public class WixCategory : BaseData
    {
        public WixCategory() { }

        public WixCategory(WixCategoryDataOutput input)
        {
            if (input == null)
                return;

            WixCategoryId = input.Id;
            Title = input.Label;
            Slug = input.Slug;
            ImageUrl = input.CoverMedia?.Image?.Url;
            PostCount = input.PostCount;
            Rank = input.Rank;
        }

        public WixCategory(WixCategoryDataOutput input, string id)
        {
            if (input == null)
                return;

            Id = id;
            WixCategoryId = input.Id;
            Title = input.Label;
            Slug = input.Slug;
            ImageUrl = input.CoverMedia?.Image?.Url;
            PostCount = input.PostCount;
            Rank = input.Rank;
        }

        public string WixCategoryId { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string ImageUrl { get; set; }
        public int PostCount { get; set; }
        public int Rank { get; set; }
    }
}
