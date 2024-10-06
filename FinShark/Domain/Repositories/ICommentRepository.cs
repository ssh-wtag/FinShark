using Domain.Helpers;
using Domain.Models;

namespace Domain.Repositories
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync(CommentQueryObject query);

        Task<Comment?> GetByIdAsync(int id);

        Task<Comment> CreateAsync(Comment comment);

        Task<Comment?> UpdateAsync(int commentID, Comment updatedComment);

        Task<Comment?> DeleteAsync(int commentId);
    }
}
