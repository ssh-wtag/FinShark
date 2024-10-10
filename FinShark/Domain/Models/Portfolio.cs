using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table("Portfolios")]
    public class Portfolio
    {
        // Junction Table
        public string AppUserId { get;set;}

        public int StockID { get;set;}


        public AppUser AppUser { get;set;}

        public Stock Stock { get;set;}
    }
}
