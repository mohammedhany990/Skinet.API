using MediatR;
using Skinet.API.Features.Favorites.Models;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Favorites.Queries.GetUserFavoritesQuery
{
    public class GetUserFavoritesQuery : IRequest<BaseResponse<List<FavoriteItemModel>>>
    {
        
    }
}
