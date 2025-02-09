using AutoMapper;
using MediatR;
using Org.BouncyCastle.Bcpg;
using Skinet.Core.Entities.Basket;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;

namespace Skinet.API.Features.Baskets.Queries.Get
{
    public class GetByIdBasketHandler : IRequestHandler<GetByIdBasketQuery, BaseResponse<BasketResponse>>
    {
        private readonly IBasketService _basketService;
        private readonly IMapper _mapper;

        public GetByIdBasketHandler(IBasketService basketService, IMapper mapper)
        {
            _basketService = basketService;
            _mapper = mapper;
        }

        public async Task<BaseResponse<BasketResponse>> Handle(GetByIdBasketQuery request, CancellationToken cancellationToken)
        {
            var basket = await _basketService.GetBasketAsync(request.Id);

            var mappedBasket = _mapper.Map<BasketResponse>(basket ?? new CustomerBasket(request.Id));

            return new BaseResponse<BasketResponse>
            {
                Data = mappedBasket,
                StatusCode = 200,
                Count = 1,
                Success = true,
                Message = basket is not null ? "Found Successfully." : "Basket not found, returning an empty one."
            };

        }
    }
}
