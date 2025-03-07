using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skinet.API.DTOs.Identity;
using Skinet.API.Features.Users.Commands.UpdateAddress;
using Skinet.API.Features.Users.Queries;
using Skinet.Core.Helper;

namespace Skinet.API.Controllers
{
    [ApiVersion("1.0")]
    [Authorize]
    public class UsersController : ApiBaseController
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-address")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<BaseResponse<AddressModel>>> GetUserAddress()
        {
            var response = await _mediator.Send(new GetUserAddressQuery());
            return Ok(response);
        }

        [HttpPost("update-address")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<BaseResponse<AddressModel>>> UpdateUserAddress(UpdateAddressCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
