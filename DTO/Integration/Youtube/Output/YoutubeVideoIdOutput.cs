namespace DTO.Integration.Youtube.Output
{
    public class YoutubeVideoIdOutput
    {
        public YoutubeVideoIdOutput() { }
        public YoutubeVideoIdOutput(string videoId) => VideoId = videoId;
        public string Kind { get; set; }
        public string VideoId { get; set; }
    }
}
