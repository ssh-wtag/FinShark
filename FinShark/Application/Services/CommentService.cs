using Application.Interfaces;
using Application.Mappers;
using Domain.DTOs.Comment;
using Domain.Helpers;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class CommentService : ICommentService
    {
        #region Initialization

        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        #endregion



        #region Implementation

        public async Task<List<Comment>> GetAllAsync(CommentQueryObject query)
        {
            var comments = await _commentRepository.GetAllAsync();

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
            return await _commentRepository.GetByIdAsync(id);
        }



        public async Task<Comment> CreateAsync(string appUserId, int stockId, CreateCommentRequestDTO commentRequestDTO)
        {
            var comment = commentRequestDTO.ToCommentFromCreateCommentRequestDTO(stockId);
            comment.AppUserId = appUserId;

            return await _commentRepository.CreateAsync(comment);
        }



        public async Task<Comment?> UpdateAsync(int commentID, Comment updatedComment)
        {
            var existingComment = await _commentRepository.GetByIdAsync(commentID);

            if (existingComment == null)
                return null;

            existingComment.Title = updatedComment.Title;
            existingComment.Content = updatedComment.Content;

            return await _commentRepository.UpdateAsync(existingComment);
        }



        public async Task<Comment?> DeleteAsync(int commentId)
        {
            var existingComment = await _commentRepository.GetByIdAsync(commentId);

            if (existingComment == null)
                return null;

            return await _commentRepository.DeleteAsync(existingComment);
        }

        #endregion
    }
}
