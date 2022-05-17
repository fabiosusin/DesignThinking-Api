namespace DTO.Integration.Youtube.Output
{
    public class YoutubeVideoSnippetOutput
    {
        public string PublishedAt { get; set; }
        public string ChannelId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public YoutubeThumbnailsVideo Thumbnails { get; set; }
        public YoutubeVideoIdOutput ResourceId { get; set; }
    }


    public class YoutubeThumbnailsVideo
    {
        public YoutubeThumbnailData Medium { get; set; }
        public YoutubeThumbnailData High { get; set; }
        public YoutubeThumbnailData Standard { get; set; }
        public YoutubeThumbnailData Maxres { get; set; }
    }

    public class YoutubeThumbnailData
    {
        public string Url { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
