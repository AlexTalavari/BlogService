using System.Collections.Generic;
using System.Threading.Tasks;
using BlogService.DataModels;

namespace BlogService.Abstractions.Repositories
{
    public interface ICommentRepository
    {
        Task AddComment(Comment item);
        Task<IEnumerable<Comment>> GetAllComments();

        Task<Comment> GetComment(string id);

        Task<IEnumerable<Comment>> GetPostComments(string id);

        Task<bool> RemoveAllComments();

        Task<bool> RemoveComment(string id);

        Task<bool> UpdateComment(string id, string body);

        Task<bool> UpdateComment(string id, Comment item);
    }
}