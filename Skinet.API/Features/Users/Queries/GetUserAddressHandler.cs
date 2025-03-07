using MediatR;
using Skinet.API.DTOs.Identity;
using Skinet.Core.Helper;
using Skinet.Service.Abstracts;
using System.Security.Claims;

namespace Skinet.API.Features.Users.Queries
{
    public class GetUserAddressHandler : IRequestHandler<GetUserAddressQuery, BaseResponse<AddressModel>>
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetUserAddressHandler(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<BaseResponse<AddressModel>> Handle(GetUserAddressQuery request, CancellationToken cancellationToken)
        {
            var email = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                return new BaseResponse<AddressModel>(404, false, "User not authenticated.");
            }

            var address = await _userService.GetUserAddressAsync(email);

            if (address is null)
            {
                return new BaseResponse<AddressModel>(404, false, "Address not found for the user.");
            }

            var addressModel = new AddressModel
            {
                FirstName = address.FirstName,
                LastName = address.LastName,
                City = address.City,
                State = address.State,
                Street = address.Street,
                ZipCode = address.ZipCode
            };

            return new BaseResponse<AddressModel>(200, true, 1, addressModel, "Address retrieved successfully.");
        }
    }
}
