namespace BlogService.DataModels
{
    public class HeaderImage
    {
        public long ImageSize { get; set; } = 0L;
        public string ThumbnailUrl { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}