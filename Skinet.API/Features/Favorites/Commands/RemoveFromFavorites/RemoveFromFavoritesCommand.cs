using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Favorites.Commands.RemoveFromFavorites
{
    public class RemoveFromFavoritesCommand : IRequest<BaseResponse<string>>
    {

        public int ProductId { get; set; }

        public RemoveFromFavoritesCommand(int productId)
        {

            ProductId = productId;
        }
    }
}
