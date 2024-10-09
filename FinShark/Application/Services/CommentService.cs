using Application.Interfaces;
using Domain.Helpers;
using Domain.Models;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;


        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }


        public Task<List<Comment>> GetAllAsync(CommentQueryObject query)
        {
            throw new NotImplementedException();
        }


        public Task<Comment?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }


        public Task<Comment> CreateAsync(Comment comment)
        {
            throw new NotImplementedException();
        }


        public Task<Comment?> UpdateAsync(int commentID, Comment updatedComment)
        {
            throw new NotImplementedException();
        }


        public async Task<Comment?> DeleteAsync(int commentId)
        {
            var existingComment = await _commentRepository.GetByIdAsync(commentId);

            if (existingComment == null)
                return null;

            return await _commentRepository.DeleteAsync(existingComment);
        }
    }
}
