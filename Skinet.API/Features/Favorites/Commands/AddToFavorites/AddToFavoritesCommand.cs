using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Favorites.Commands.Create
{
    public class AddToFavoritesCommand : IRequest<BaseResponse<string>>
    {
        public int ProductId { get; set; }
    }
}
