using Skinet.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skinet.Core.Entities.Basket;
using Skinet.Core.Interfaces;
using StackExchange.Redis;

namespace Skinet.Service.Implementation
{
    public class BasketService : IBasketService
    {
        private readonly IUnitOfWork _unitOfWork;
      
        public BasketService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
           var basket =  await  _unitOfWork.BasketRepository.GetBasketAsync(basketId);
           return basket;
        }

        public async Task<CustomerBasket?> UpdateOrCreateBasketAsync(CustomerBasket basket)
        {
           return await _unitOfWork.BasketRepository.UpdateOrCreateBasketAsync(basket);
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _unitOfWork.BasketRepository.DeleteBasketAsync(basketId);
        }
    }
}
