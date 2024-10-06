using Domain.DTOs.Comment;
using Domain.Models;

namespace Application.Mappers
{
    public static class CommentMapper
    {
        public static CommentDTO ToCommentDTOFromComment(this Comment commentModel)
        {
            return new CommentDTO
            {
                Id = commentModel.Id,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreatedBy = commentModel.AppUser.UserName,
                CreatedOn = commentModel.CreatedOn,
                StockId = commentModel.StockId,
            };
        }


        public static Comment ToCommentFromCreateCommentRequestDTO(this CreateCommentRequestDTO createCommentRequest, int stockId)
        {
            return new Comment
            {
                Title = createCommentRequest.Title,
                Content = createCommentRequest.Content,
                StockId = stockId
            };
        }


        public static Comment ToCommentFromUpdateRequestDTO(this UpdateCommentRequestDTO updateCommentRequestDTO)
        {
            return new Comment
            {
                Title = updateCommentRequestDTO.Title,
                Content = updateCommentRequestDTO.Content
            };
        }
    }
}
