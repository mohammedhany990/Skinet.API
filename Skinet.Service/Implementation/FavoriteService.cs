using Skinet.Core.Entities;
using Skinet.Core.Specifications.FaviroteList;
using Skinet.Repository.Interfaces;
using Skinet.Service.Interfaces;

namespace Skinet.Service.Implementation
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FavoriteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<FavoriteItem>> GetUserFavoritesAsync(string userId)
        {
            var spec = new FavoriteItemsByUserSpecification(userId);
            return await _unitOfWork.Repository<FavoriteItem>().GetAllWithSpecAsync(spec);
        }

        public async Task<string> AddToFavoritesAsync(string userId, int productId)
        {
            //if (string.IsNullOrEmpty(userId))
            //    return "User ID is required.";

            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productId);
            if (product is null)
                return "Product not found.";


            var favoriteList = await _unitOfWork.Repository<FavoriteList>()
                .FirstOrDefaultAsync(i => i.UserId == userId) ?? new FavoriteList
                {
                    CreatedDate = DateTime.UtcNow,
                    UserId = userId
                };

            // If its Id is zero, so I will create a new one
            if (favoriteList.Id == 0)
            {
                await _unitOfWork.Repository<FavoriteList>().AddAsync(favoriteList);
                await _unitOfWork.CompleteAsync();
            }


            bool exists = await _unitOfWork.Repository<FavoriteItem>()
                .ExistsAsync(f => f.FavoriteListId == favoriteList.Id && f.ProductId == productId);

            if (exists)
                return "Item already exists in favorites.";


            var favoriteItem = new FavoriteItem
            {
                FavoriteListId = favoriteList.Id,
                ProductId = productId,
                ProductName = product.Name,
                CreatedAt = DateTime.UtcNow,

            };

            await _unitOfWork.Repository<FavoriteItem>().AddAsync(favoriteItem);
            await _unitOfWork.CompleteAsync();

            return "Item added to favorites successfully.";
        }



        public async Task<string> RemoveFromFavoritesAsync(string userId, int productId)
        {
            var favoriteList = await _unitOfWork.Repository<FavoriteList>()
                .FirstOrDefaultAsync(f => f.UserId == userId);

            if (favoriteList is null)
                return "Favorite list not found.";

            var favoriteItem = await _unitOfWork.Repository<FavoriteItem>()
                .FirstOrDefaultAsync(f => f.FavoriteListId == favoriteList.Id && f.ProductId == productId);

            if (favoriteItem is null)
                return "Item not found in favorites.";

            _unitOfWork.Repository<FavoriteItem>().Delete(favoriteItem);
            await _unitOfWork.CompleteAsync();

            return "Item removed from favorites.";
        }

        public async Task<string> ClearFavoritesAsync(string userId)
        {
            var favoriteList = await _unitOfWork.Repository<FavoriteList>()
                .FirstOrDefaultAsync(f => f.UserId == userId);

            if (favoriteList is null)
                return "Favorite list not found.";

            var favoriteItems = await _unitOfWork.Repository<FavoriteItem>()
                .GetAllAsync(f => f.FavoriteListId == favoriteList.Id);

            if (!favoriteItems.Any())
                return "No items in favorites.";

            await _unitOfWork.Repository<FavoriteItem>()
                .DeleteWhereAsync(f => f.FavoriteListId == favoriteList.Id);

            await _unitOfWork.CompleteAsync();

            return "All favorites cleared.";
        }
    }

}