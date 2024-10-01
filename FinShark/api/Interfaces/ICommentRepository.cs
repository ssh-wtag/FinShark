using api.DTOs.Comment;
using api.Models;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();

        Task<Comment?> GetByIdAsync(int id);

        Task<Comment> CreateAsync(Comment comment);

        Task<Comment?> UpdateAsync(int commentID, Comment updatedComment);

        Task<Comment?> DeleteAsync(int commentId);
    }
}
