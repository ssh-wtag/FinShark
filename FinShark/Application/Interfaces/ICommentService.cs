using Domain.Helpers;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICommentService
    {
        Task<List<Comment>> GetAllAsync(CommentQueryObject query);

        Task<Comment?> GetByIdAsync(int id);

        Task<Comment> CreateAsync(Comment comment);

        Task<Comment?> UpdateAsync(int commentID, Comment updatedComment);

        Task<Comment?> DeleteAsync(int commentId);
    }
}
