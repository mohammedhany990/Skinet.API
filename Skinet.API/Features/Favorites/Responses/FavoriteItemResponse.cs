using Skinet.API.Features.Products.Responses;

namespace Skinet.API.Features.Favorites.Responses
{
    public class FavoriteItemResponse
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public int FavoriteListId { get; set; }
        public ProductResponse Product { get; set; }
    }
}
