using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    [Table("Comments")]
    public class Comment
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        //One-to-One
        public int? StockId { get; set; }

        //This is the navigation property, it'll let us dot into it later.
        public Stock? Stock { get; set; }

        //One-to-One
        public string AppUserId { get; set; }

        //Navigation Property Again
        public AppUser AppUser { get; set; }
    }
}