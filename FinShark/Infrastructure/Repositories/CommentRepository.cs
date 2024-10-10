using Infrastructure.Data;
using Domain.Helpers;
using Domain.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        #region Initialization

        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }

        #endregion



        #region Implementation

        public async Task<IQueryable<Comment>> GetAllAsync()
        {
            return _context.Comments.Include(s => s.AppUser).AsQueryable();
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



        public async Task<Comment?> UpdateAsync(Comment comment)
        {
            await _context.SaveChangesAsync();
            return comment;
        }



        public async Task<Comment?> DeleteAsync(Comment comment)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return comment;
        }

        #endregion
    }
}
