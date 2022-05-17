namespace DTO.Mobile.News.Input
{
    public class AppNewsFiltersInput
    {
        public bool OnlyLinked { get; set; }
        public string AllyId { get; set; }
        public string Title { get; set; }
        public string WixCategoryId { get; set; }
        public string WixTagId { get; set; }
        public string Ids { get; set; }
    }
}
