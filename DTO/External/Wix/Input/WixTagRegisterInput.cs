using System;

namespace DTO.External.Wix.Input
{
    public class WixTagRegisterInput
    {
        public string Id { get; set; }
        public string Label { get; set; }
        public string Slug { get; set; }
        public int PublicationCount { get; set; }
        public int PostCount { get; set; }
        public string Language { get; set; }
        public int PublishedPostCount { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
