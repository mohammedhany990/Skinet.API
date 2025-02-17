using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Favorites.Commands.ClearFavorites
{
    public class ClearFavoritesCommand : IRequest<BaseResponse<string>>
    {
    }
}
