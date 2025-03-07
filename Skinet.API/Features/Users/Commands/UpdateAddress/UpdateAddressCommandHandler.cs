using MediatR;
using Skinet.Core.Entities.Identity;
using Skinet.Core.Helper;
using Skinet.Service.Abstracts;
using System.Security.Claims;

namespace Skinet.API.Features.Users.Commands.UpdateAddress
{
    public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, BaseResponse<string>>
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateAddressCommandHandler(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<BaseResponse<string>> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            var email = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                return new BaseResponse<string>(404, false, "User not authenticated.");
            }
            var address = new Address()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                City = request.City,
                State = request.State,
                Street = request.Street,
                ZipCode = request.ZipCode
            };

            var result = await _userService.UpdateAddressAsync(address, email);
            bool isSuccess = "Address updated successfully." == result;
            return new BaseResponse<string>
            {
                Message = result,
                StatusCode = isSuccess ? 200 : 400,
                Success = isSuccess
            };
        }
    }
}
