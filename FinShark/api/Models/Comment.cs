namespace api.Models
{
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
    }
}