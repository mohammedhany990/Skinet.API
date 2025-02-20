namespace Skinet.Core.Entities
{
    public class FavoriteItem : BaseEntity
    {
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ProductName { get; set; }

        // Navigation properties
        public int FavoriteListId { get; set; }
        public FavoriteList FavoriteList { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
