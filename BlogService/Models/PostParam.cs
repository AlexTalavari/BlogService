using System.Collections.Generic;

namespace BlogService.Models
{
    public class PostParam
    {
        public string Body { get; set; } = string.Empty;
        public HeaderImage Header { get; set; } = new HeaderImage();
        public string Id { get; set; } = string.Empty;

        public List<string> Tags { get; set; } = new List<string>();

        public string Title { get; set; } = string.Empty;
    }
}