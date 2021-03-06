﻿namespace BlogService.ViewModels
{
    public class CommentViewModel
    {
        public string Body { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        //external Id
        public string Id { get; set; }

        public string LastName { get; set; } = string.Empty;

        public string Website { get; set; } = string.Empty;

        public string PostId { get; set; }
    }
}