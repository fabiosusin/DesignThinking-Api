using DTO.External.Wix.Database;
using System.Collections.Generic;

namespace DTO.Mobile.News.Output
{
    public class AppNewsOutput : WixPost
    {
        public AppNewsOutput(WixPost input)
        {
            if (input == null)
                return;

            Id = input.Id;
            WixPostId = input.Id;
            Title = input.Title;
            Content = input.Content;
            Slug = input.Slug;
            ImageUrl = input.ImageUrl;
            MemberId = input.MemberId;
            ContentId = input.ContentId;
            MostRecentContributorId = input.MostRecentContributorId;
            WixCategoryIds = input.WixCategoryIds;
            WixTagIds = input.WixTagIds;
            Featured = input.Featured;
            CommentingEnabled = input.CommentingEnabled;
            MinutesToRead = input.MinutesToRead;
            PublishedDate = input.PublishedDate;
            LastUpdate = input.LastUpdate;
            Paragraphs = input.Paragraphs;
        }

        public IEnumerable<MobileNewsCategory> Categories { get; set; }
        public IEnumerable<MobileNewsCategory> Tags { get; set; }

    }

    public class MobileNewsCategory
    {
        public MobileNewsCategory(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; set; }
        public string Name { get; set; }
    }
}
