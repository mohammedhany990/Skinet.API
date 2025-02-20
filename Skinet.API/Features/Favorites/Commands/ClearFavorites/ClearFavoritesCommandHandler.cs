using MediatR;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;
using System.Security.Claims;

namespace Skinet.API.Features.Favorites.Commands.ClearFavorites
{
    public class ClearFavoritesHandler : IRequestHandler<ClearFavoritesCommand, BaseResponse<string>>
    {
        private readonly IFavoriteService _favoriteService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClearFavoritesHandler(IFavoriteService favoriteService, IHttpContextAccessor httpContextAccessor)
        {
            _favoriteService = favoriteService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<string>> Handle(ClearFavoritesCommand request, CancellationToken cancellationToken)
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

            var result = await _favoriteService.ClearFavoritesAsync(userId);

            bool success = result == "All favorites cleared.";
            int statusCode = success ? 200 : 400;

            return new BaseResponse<string>
            {
                Success = success,
                Message = result,
                StatusCode = statusCode

            };
        }
    }

}
