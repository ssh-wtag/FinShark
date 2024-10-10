using Domain.DTOs.Comment;
using api.Extensions;
using Domain.Helpers;
using Application.Mappers;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        #region Initialization

        private readonly ICommentService _commentService;
        private readonly IStockService _stockService;
        private readonly UserManager<AppUser> _userManager;

        public CommentController(ICommentService commentService, IStockService stockService, UserManager<AppUser> userManager)
        {
            _commentService = commentService;
            _stockService = stockService;
            _userManager = userManager;
        }

        #endregion



        #region Implementation

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CommentQueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var comments = await _commentService.GetAllAsync(query);
            var commentsDTO = comments.Select(x => x.ToCommentDTOFromComment());

            return Ok(commentsDTO);
        }



        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentService.GetByIdAsync(id);

            if (comment == null)
                return NotFound();

            return Ok(comment.ToCommentDTOFromComment());
        }



        [HttpPost("{stockId:int}")]
        [Authorize]
        public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentRequestDTO commentRequestDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _stockService.StockExistsAsync(stockId))
                return BadRequest("Stock Does Not Exist.");

            var username = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(username);

            var comment = await _commentService.CreateAsync(appUser.Id, stockId, commentRequestDTO);

            return CreatedAtAction(nameof(GetById), new { id = comment.Id}, comment.ToCommentDTOFromComment());
        }



        [HttpPut("{commentId:int}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int commentId, [FromBody] UpdateCommentRequestDTO updatedCommentDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var username = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(username);
            var comment = await _commentService.GetByIdAsync(commentId);

            if (comment == null)
                return NotFound("Comment Not Found.");

            if (comment.AppUserId != appUser.Id)
                return BadRequest($"Comment Does Not Belong to {username}");

            comment = await _commentService.UpdateAsync(commentId, updatedCommentDTO.ToCommentFromUpdateRequestDTO());

            return Ok(comment.ToCommentDTOFromComment());
        }



        [HttpDelete]
        [Authorize]
        [Route("{commentId:int}")]
        public async Task<IActionResult> Delete([FromRoute] int commentId)
        {
            var username = User.GetUserName();
            var appUser = await _userManager.FindByNameAsync(username);
            var comment = await _commentService.GetByIdAsync(commentId);

            if (comment == null)
                return NotFound("This Comment Does Not Exist.");

            if (comment.AppUserId != appUser.Id)
                return BadRequest($"Comment Does Not Belong to {username}");

            comment = await _commentService.DeleteAsync(commentId);


            return Ok(comment.ToCommentDTOFromComment());
        }

        #endregion
    }
}
