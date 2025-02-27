﻿using MediatR;
using Skinet.API.Features.Products.Responses;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Products.Queries.GetById
{
    public class GetByIdProductQuery : IRequest<BaseResponse<ProductResponse>>
    {
        public GetByIdProductQuery(int productId)
        {
            ProductId = productId;
        }
        public int ProductId { get; set; }
    }
}
