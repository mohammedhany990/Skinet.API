using AutoMapper;
using MediatR;
using Skinet.Core.Entities.Basket;
using Skinet.Core.Helper;
using Skinet.Service.Interfaces;

namespace Skinet.API.Features.Baskets.Commands.Create
{
    public class CreateBasketCommandHandler : IRequestHandler<CreateBasketCommand, BaseResponse<string>>
    {
        private readonly IBasketService _basketService;
        private readonly IMapper _mapper;

        public CreateBasketCommandHandler(IBasketService basketService, IMapper mapper)
        {
            _basketService = basketService;
            _mapper = mapper;
        }
        public async Task<BaseResponse<string>> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
        {
            var mappedBasket = _mapper.Map<CustomerBasket>(request);

            var basket = await _basketService.UpdateOrCreateBasketAsync(mappedBasket);

            if (basket is null || string.IsNullOrEmpty(basket.Id))
            {
               
                return new BaseResponse<string>(400, false, "Failed to create basket");
            }

            return new BaseResponse<string>(200, true, $"Basket created successfully with ID: {basket.Id}");

        }
    }
}
