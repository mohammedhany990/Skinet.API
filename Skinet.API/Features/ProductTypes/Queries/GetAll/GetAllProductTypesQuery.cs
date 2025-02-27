using MediatR;
using Skinet.API.Features.ProductTypes.Responses;
using Skinet.Core.Helper;

namespace Skinet.API.Features.ProductTypes.Queries.List
{
    public class GetAllProductTypesQuery : IRequest<BaseResponse<List<ProductTypeResponse>>>
    {

    }
}
