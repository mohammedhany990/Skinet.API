using AutoMapper;
using MediatR;
using Skinet.API.Features.Favorites.Responses;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;
using System.Security.Claims;

namespace Skinet.API.Features.Favorites.Queries.GetUserFavoritesQuery
{
    public class GetUserFavoritesHandler : IRequestHandler<GetUserFavoritesQuery, BaseResponse<List<FavoriteItemResponse>>>
    {
        private readonly IFavoriteService _favoriteService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public GetUserFavoritesHandler(IFavoriteService favoriteService, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _favoriteService = favoriteService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<BaseResponse<List<FavoriteItemResponse>>> Handle(GetUserFavoritesQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return new BaseResponse<List<FavoriteItemResponse>>
                {
                    Success = false,
                    Message = "Unauthorized. User ID is missing.",
                    StatusCode = 401
                };
            }

            var favoriteItems = await _favoriteService.GetUserFavoritesAsync(userId);

            if (favoriteItems is null || !favoriteItems.Any())
            {
                return new BaseResponse<List<FavoriteItemResponse>>
                {
                    Success = false,
                    Message = "No favorite items found.",
                    Data = new List<FavoriteItemResponse>(),
                    StatusCode = 404
                };
            }


            var favoriteItemModels = _mapper.Map<List<FavoriteItemResponse>>(favoriteItems);

            return new BaseResponse<List<FavoriteItemResponse>>
            {
                Success = true,
                Message = "Favorites retrieved successfully.",
                Data = favoriteItemModels,
                StatusCode = 200,
                Count = favoriteItemModels.Count
            };
        }
    }
}
