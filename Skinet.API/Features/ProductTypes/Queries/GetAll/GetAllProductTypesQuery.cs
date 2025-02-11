using MediatR;
using Skinet.API.Features.ProductTypes.Models;
using Skinet.Core.Helper;

namespace Skinet.API.Features.ProductTypes.Queries.List
{
    public class GetAllProductTypesQuery:IRequest<BaseResponse<List<ProductTypeModel>>>
    {

    }
}
