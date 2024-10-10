using System.ComponentModel.DataAnnotations;

namespace Domain.Helpers
{
    public class StockQueryObject
    {
        [MaxLength(10, ErrorMessage = "Symbols Are Not Over 10 Characters")]
        public string? Symbol { get; set; } = null;

        [MaxLength(200, ErrorMessage = "Company Names Are Not Over 200 Characters")]
        public string? CompanyName { get; set; } = null;


        // For Sorting
        public string? SortBy { get; set; } = null;
        public bool IsDescending { get; set; } = false;


        // For Pagination
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
