namespace Skinet.API.Features.Favorites.Models
{
    public class FavouriteListModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastUpdated { get; set; }
        public ICollection<FavoriteItemModel> FavouriteItems { get; set; } = new List<FavoriteItemModel>();



    }
}
