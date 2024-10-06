using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Stock
{
    public class UpdateStockRequestDTO
    {
        [Required]
        [MinLength(1, ErrorMessage = "Symbol Cannot Be Empty.")]
        [MaxLength(10, ErrorMessage = "Symbol Cannot Be Over 10 Characters.")]
        public string Symbol { get; set; } = string.Empty;


        [Required]
        [MinLength(1, ErrorMessage = "Company Name Cannot Be Empty.")]
        [MaxLength(100, ErrorMessage = "Company Name Cannot Be Over 100 Characters.")]
        public string CompanyName { get; set; } = string.Empty;


        [Required]
        [Range(1, 100000000)]
        public decimal Purchase { get; set; }


        [Required]
        [Range(0.001, 100)]
        public decimal LastDiv { get; set; }


        [Required]
        [MaxLength(50, ErrorMessage = "Industry Cannot Be Over 50 Characters.")]
        public string Industry { get; set; } = string.Empty;


        [Required]
        [Range(1, 5000000000)]
        public long MarketCap { get; set; }
    }
}
