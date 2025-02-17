using MediatR;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;
using System.Security.Claims;

namespace Skinet.API.Features.Favorites.Commands.RemoveFromFavorites
{
    public class RemoveFromFavoritesHandler : IRequestHandler<RemoveFromFavoritesCommand, BaseResponse<string>>
    {
        private readonly IFavoriteService _favoriteService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RemoveFromFavoritesHandler(IFavoriteService favoriteService, IHttpContextAccessor httpContextAccessor)
        {
            _favoriteService = favoriteService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<string>> Handle(RemoveFromFavoritesCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return new BaseResponse<string>
                {
                    Success = false,
                    Message = "Unauthorized. User ID is missing.",
                    StatusCode = 401
                };
            }
            
            var result = await _favoriteService.RemoveFromFavoritesAsync(userId, request.ProductId);

            bool success = result == "Item removed from favorites.";
            int statusCode = success ? 200 : 400;

            return new BaseResponse<string>
            {
                Success = success ,
                Message = result,
                StatusCode = statusCode
            };
        }
    }

}
