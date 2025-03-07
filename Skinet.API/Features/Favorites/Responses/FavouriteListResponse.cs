namespace Skinet.API.Features.Favorites.Responses
{
    public class FavouriteListResponse
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastUpdated { get; set; }
        public ICollection<FavoriteItemResponse> FavouriteItems { get; set; } = new List<FavoriteItemResponse>();



    }
}
