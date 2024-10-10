using System.ComponentModel.DataAnnotations;

namespace Domain.Helpers
{
    public class CommentQueryObject
    {
        [MaxLength(200, ErrorMessage = "Title Cannot Be Over 200 Characters")]
        public string? Title { get; set; } = null;

        [MaxLength(400, ErrorMessage = "Content Cannot Be Over 400 Characters")]
        public string? Content { get; set; } = null;


        // For Sorting
        public string? SortBy { get; set; } = null;

        public bool IsDescending { get; set; } = false;


        // For Pagination
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 20;
    }
}
