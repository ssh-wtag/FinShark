using Domain.Helpers;
using Domain.Models;

namespace Domain.Repositories
{
    public interface ICommentRepository
    {
        Task<IQueryable<Comment>> GetAllAsync();

        Task<Comment?> GetByIdAsync(int id);

        Task<Comment> CreateAsync(Comment comment);

        Task<Comment?> UpdateAsync(Comment comment);

        Task<Comment?> DeleteAsync(Comment comment);
    }
}
