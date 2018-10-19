using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlogService.Models;

namespace BlogService.Abstractions
{
    public interface IPostRepository
    {
        Task AddPost(Post item);
        Task<IEnumerable<Post>> GetAllPosts();

        Task<Post> GetPost(string id);

        Task<IEnumerable<Post>> GetPost(string bodyText, DateTime updatedFrom, long headerSizeLimit);

        Task<bool> RemoveAllPosts();

        Task<bool> RemovePost(string id);

        Task<bool> UpdatePost(string id, string body);

        Task<bool> UpdatePost(string id, Post item);
    }
}