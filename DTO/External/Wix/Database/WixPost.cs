using DTO.External.Wix.Enum;
using DTO.General.Base.Database;
using DTO.Integration.Wix.Post.Output;
using System;
using System.Collections.Generic;

namespace DTO.External.Wix.Database
{
    public class WixPost : BaseData
    {
        public WixPost() { }

        public WixPost(WixPostDataOutput input, List<WixPostParagraphData> paragraphs)
        {
            if (input == null)
                return;

            WixPostId = input.Id;
            Title = input.Title;
            Content = input.ContentText;
            Slug = input.Slug;
            ImageUrl = input.CoverMedia?.Image?.Url;
            MemberId = input.MemberId;
            ContentId = input.ContentId;
            MostRecentContributorId = input.MostRecentContributorId;
            WixCategoryIds = input.CategoryIds;
            Featured = input.Featured;
            CommentingEnabled = input.CommentingEnabled;
            MinutesToRead = input.MinutesToRead;
            PublishedDate = input.FirstPublishedDate;
            LastUpdate = input.LastPublishedDate;
            WixTagIds = input.TagIds;
            Paragraphs = paragraphs;
        }

        public WixPost(string id, WixPostDataOutput input, List<WixPostParagraphData> paragraphs)
        {
            if (input == null)
                return;

            Id = id;
            WixPostId = input.Id;
            Title = input.Title;
            Content = input.ContentText;
            Slug = input.Slug;
            ImageUrl = input.CoverMedia?.Image?.Url;
            MemberId = input.MemberId;
            ContentId = input.ContentId;
            MostRecentContributorId = input.MostRecentContributorId;
            WixCategoryIds = input.CategoryIds;
            Featured = input.Featured;
            CommentingEnabled = input.CommentingEnabled;
            MinutesToRead = input.MinutesToRead;
            PublishedDate = input.FirstPublishedDate;
            LastUpdate = input.LastPublishedDate;
            WixTagIds = input.TagIds;
            Paragraphs = paragraphs;
        }

        public string WixPostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Slug { get; set; }
        public string ImageUrl { get; set; }
        public string MemberId { get; set; }
        public string ContentId { get; set; }
        public string MostRecentContributorId { get; set; }
        public List<WixPostParagraphData> Paragraphs { get; set; }
        public List<string> WixCategoryIds { get; set; }
        public List<string> WixTagIds { get; set; }
        public bool Featured { get; set; }
        public bool CommentingEnabled { get; set; }
        public int MinutesToRead { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime LastUpdate { get; set; }
    }

    public class WixPostParagraphData
    {
        public WixPostParagraphData(WixPostParagraphType type) => Type = type;
        public WixPostParagraphData(WixPostParagraphType type, string data)
        {
            Data = data;
            Type = type;
        }

        public string Data { get; set; }
        public WixPostParagraphType Type { get; set; }
    }

}
