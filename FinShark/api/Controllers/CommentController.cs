using api.DTOs.Comment;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        #region Initialization

        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;

        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
        }

        #endregion

        #region Implementation

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CommentQueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comments = await _commentRepository.GetAllAsync(query);
            var commentsDTO = comments.Select(x => x.ToCommentDTOFromComment());

            return Ok(commentsDTO);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);

            if (comment == null)
                return NotFound();

            return Ok(comment.ToCommentDTOFromComment());
        }


        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentRequestDTO commentRequestDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _stockRepository.StockExists(stockId))
                return BadRequest("Stock Does Not Exist.");

            var comment = commentRequestDTO.ToCommentFromCreateCommentRequestDTO(stockId);

            await _commentRepository.CreateAsync(comment);

            return CreatedAtAction(nameof(GetById), new { id = comment.Id}, comment.ToCommentDTOFromComment());
        }


        [HttpPut("{commentId:int}")]
        public async Task<IActionResult> Update([FromRoute] int commentId, [FromBody] UpdateCommentRequestDTO updatedCommentDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await _commentRepository.UpdateAsync(commentId, updatedCommentDTO.ToCommentFromUpdateRequestDTO());

            if(comment == null)
                return NotFound("Comment Not Found.");

            return Ok(comment.ToCommentDTOFromComment());
        }


        [HttpDelete]
        [Route("{commentId:int}")]
        public async Task<IActionResult> Delete([FromRoute] int commentId)
        {
            var removedComment = await _commentRepository.DeleteAsync(commentId);

            if (removedComment == null)
                return NotFound("This Comment Does Not Exist.");

            return Ok(removedComment.ToCommentDTOFromComment());
        }

        #endregion
    }
}
