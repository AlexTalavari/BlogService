using System.Collections.Generic;
using BlogService.DataModels;

namespace BlogService.ViewModels
{
    public class PostViewModel
    {
        public string Body { get; set; } = string.Empty;
        public HeaderImage Header { get; set; } = new HeaderImage();
        public string Id { get; set; } = string.Empty;

        public List<string> Tags { get; set; } = new List<string>();

        public string Title { get; set; } = string.Empty;
    }
}