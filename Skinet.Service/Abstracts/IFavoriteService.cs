using Skinet.Core.Entities;

namespace Skinet.Service.Interfaces
{

    public interface IFavoriteService
    {
        Task<List<FavoriteItem>> GetUserFavoritesAsync(string userId);
        Task<string> AddToFavoritesAsync(string userId, int productId);
        Task<string> RemoveFromFavoritesAsync(string userId, int productId);
        Task<string> ClearFavoritesAsync(string userId);
    }


}
