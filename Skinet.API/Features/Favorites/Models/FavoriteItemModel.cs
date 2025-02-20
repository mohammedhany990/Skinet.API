using Skinet.API.Features.Products.Models;

namespace Skinet.API.Features.Favorites.Models
{
    public class FavoriteItemModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int FavoriteListId { get; set; }
        public ProductModel Product { get; set; }
    }
}
