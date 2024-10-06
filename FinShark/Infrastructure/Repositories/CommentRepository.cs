using Infrastructure.Data;
using Domain.Helpers;
using Domain.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }



        public async Task<List<Comment>> GetAllAsync(CommentQueryObject query)
        {
            var comments = _context.Comments.Include(s => s.AppUser).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Title))
                comments = comments.Where(s => s.Title.Contains(query.Title));

            if (!string.IsNullOrWhiteSpace(query.Content))
                comments = comments.Where(s => s.Content.Contains(query.Content));

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Title", StringComparison.OrdinalIgnoreCase))
                {
                    if (query.IsDescending)
                        comments = comments.OrderByDescending(s => s.Title);
                    else
                        comments = comments.OrderBy(s => s.Title);
                }
                else if (query.SortBy.Equals("Content", StringComparison.OrdinalIgnoreCase))
                {
                    if (query.IsDescending)
                        comments = comments.OrderByDescending(s => s.Content);
                    else
                        comments = comments.OrderBy(s => s.Content);
                }
                else if (query.SortBy.Equals("Created On", StringComparison.OrdinalIgnoreCase))
                {
                    if (query.IsDescending)
                        comments = comments.OrderByDescending(s => s.CreatedOn);
                    else
                        comments = comments.OrderBy(s => s.CreatedOn);
                }
            }

            int skipNum = (query.PageNumber - 1) * query.PageSize;

            return await comments.Skip(skipNum).Take(query.PageSize).ToListAsync();
        }



        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.Include(s => s.AppUser).FirstOrDefaultAsync(s => s.Id == id);
        }



        public async Task<Comment> CreateAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return comment;
        }



        public async Task<Comment?> UpdateAsync(int commentId, Comment updatedComment)
        {
            var existingComment = await _context.Comments.FindAsync(commentId);

            if (existingComment == null)
                return null;

            existingComment.Title = updatedComment.Title;
            existingComment.Content = updatedComment.Content;

            await _context.SaveChangesAsync();

            return existingComment;
        }



        public async Task<Comment?> DeleteAsync(int commentId)
        {
            var existingComment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

            if (existingComment == null)
                return null;

            _context.Comments.Remove(existingComment);
            await _context.SaveChangesAsync();

            return existingComment;
        }
    }
}
