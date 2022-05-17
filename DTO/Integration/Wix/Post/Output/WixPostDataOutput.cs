using DTO.Integration.Wix.Base;
using System;
using System.Collections.Generic;

namespace DTO.Integration.Wix.Post.Output
{

    public class WixPostDataOutput
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Excerpt { get; set; }
        public string ContentText { get; set; }
        public DateTime FirstPublishedDate { get; set; }
        public DateTime LastPublishedDate { get; set; }
        public string Slug { get; set; }
        public bool Featured { get; set; }
        public bool Pinned { get; set; }
        public List<string> CategoryIds { get; set; }
        public WixDataCoverMedia CoverMedia { get; set; }
        public string MemberId { get; set; }
        public List<string> Hashtags { get; set; }
        public bool CommentingEnabled { get; set; }
        public int MinutesToRead { get; set; }
        public List<string> TagIds { get; set; }
        public List<string> PricingPlans { get; set; }
        public List<string> RelatedPostIds { get; set; }
        public List<string> PricingPlanIds { get; set; }
        public string Language { get; set; }
        public string ContentId { get; set; }
        public string MostRecentContributorId { get; set; }
        public List<string> InternalCategoryIds { get; set; }
        public List<string> InternalRelatedPostIds { get; set; }
        public WixRichContent RichContent { get; set; }
    }

    public class WixRichContent
    {
        public List<WixRichContentNode> Nodes { get; set; }
    }

    public class WixRichContentNode
    {
        public string Type { get; set; }
        public WixRichContentImageData ImageData { get; set; }
        public List<WixRichContentNode_Node> Nodes { get; set; }
    }

    public class WixRichContentImageData
    {
        public WixRichContentImage Image { get; set; }
    }

    public class WixRichContentImageSrc
    {
        public string Id { get; set; }
    }

    public class WixRichContentImage
    {
        public WixRichContentImageSrc Src { get; set; }
    }

    public class WixRichContentTextData
    {
        public string Text { get; set; }
    }

    public class WixRichContentNode_Node
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public WixRichContentTextData TextData { get; set; }
    }
}
