using MediatR;
using Skinet.API.Features.Favorites.Responses;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Favorites.Queries.GetUserFavoritesQuery
{
    public class GetUserFavoritesQuery : IRequest<BaseResponse<List<FavoriteItemResponse>>>
    {

    }
}
