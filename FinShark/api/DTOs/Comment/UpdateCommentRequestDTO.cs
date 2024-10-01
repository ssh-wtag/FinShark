using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Comment
{
    public class UpdateCommentRequestDTO
    {
        [Required]
        [MinLength(1, ErrorMessage = "Title Cannot Be Empty")]
        [MaxLength(200, ErrorMessage = "Title Cannot Be Over 200 Characters")]
        public string Title { get; set; } = string.Empty;


        [Required]
        [MinLength(1, ErrorMessage = "Content Cannot Be Empty")]
        [MaxLength(400, ErrorMessage = "Content Cannot Be Over 400 Characters")]
        public string Content { get; set; } = string.Empty;
    }
}
