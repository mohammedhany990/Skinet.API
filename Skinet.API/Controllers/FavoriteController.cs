using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skinet.API.Features.Favorites.Commands.ClearFavorites;
using Skinet.API.Features.Favorites.Commands.Create;
using Skinet.API.Features.Favorites.Commands.RemoveFromFavorites;
using Skinet.API.Features.Favorites.Queries.GetUserFavoritesQuery;
using Skinet.API.Helper;
using Skinet.Core.Helper;

namespace Skinet.API.Controllers
{
    [ApiVersion("1.0")]
    [Authorize]
    public class FavoriteController : ApiBaseController
    {
        private readonly IMediator _mediator;

        public FavoriteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [CacheAttribute(300)]
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetFavorites()
        {
            var favorites = await _mediator.Send(new GetUserFavoritesQuery());
            return Ok(favorites);
        }

        [MapToApiVersion("1.0")]
        [HttpPost]
        public async Task<ActionResult<BaseResponse<string>>> AddToFavorites([FromBody] AddToFavoritesCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [MapToApiVersion("1.0")]
        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFromFavorites(int productId)
        {
            var result = await _mediator.Send(new RemoveFromFavoritesCommand(productId));
            return Ok(result);
        }


        [MapToApiVersion("1.0")]
        [HttpDelete]
        public async Task<IActionResult> ClearFavorites()
        {
            var result = await _mediator.Send(new ClearFavoritesCommand());
            return Ok(result);
        }
    }
}
