using MediatR;
using Skinet.API.Features.Favorites.Commands.Create;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;
using System.Security.Claims;

namespace Skinet.API.Features.Favorites.Commands.AddToFavorites
{
    public class AddToFavoritesHandler : IRequestHandler<AddToFavoritesCommand, BaseResponse<string>>
    {
        private readonly IFavoriteService _favoriteService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddToFavoritesHandler(IFavoriteService favoriteService, IHttpContextAccessor httpContextAccessor)
        {
            _favoriteService = favoriteService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<string>> Handle(AddToFavoritesCommand request, CancellationToken cancellationToken)
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

            var result = await _favoriteService.AddToFavoritesAsync(userId, request.ProductId);

            bool success = result == "Item added to favorites successfully.";
            int statusCode = success ? 200 : 400;

            return new BaseResponse<string>
            {
                Success = success,
                StatusCode = statusCode,
                Message = result,
                Data = result
            };
        }
    }
}