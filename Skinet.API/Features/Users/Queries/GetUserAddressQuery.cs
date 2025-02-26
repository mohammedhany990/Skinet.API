using MediatR;
using Skinet.API.DTOs.Identity;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Users.Queries
{
    public class GetUserAddressQuery : IRequest<BaseResponse<AddressModel>>
    {

    }
}
