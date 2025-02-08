﻿using MediatR;
using Skinet.API.Features.ProductBrands.Queries.Responses;
using Skinet.Core.Helper;

namespace Skinet.API.Features.ProductBrands.Queries.List
{
    public class GetAllProductBrandsQuery : IRequest<BaseResponse<List<ProductBrandResponse>>>
    {
        
    }
}
