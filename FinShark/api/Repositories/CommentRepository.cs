using api.Data;
using api.DTOs.Comment;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync();
        }


        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
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
