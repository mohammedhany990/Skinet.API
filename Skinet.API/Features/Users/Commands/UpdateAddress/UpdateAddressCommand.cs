using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Users.Commands.UpdateAddress
{
    public class UpdateAddressCommand : IRequest<BaseResponse<string>>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string ZipCode { get; set; }
    }
}
