using Skinet.Repository.Abstracts;
using Skinet.Repository.Interfaces;
using StackExchange.Redis;

namespace Skinet.Repository.Implementation
{
    public class CartRepositoryFactory : ICartRepositoryFactory
    {
        private readonly IConnectionMultiplexer _redis;

        public CartRepositoryFactory(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public ICartRepository Create(IUnitOfWork unitOfWork)
        {
            return new CartRepository(_redis, unitOfWork);
        }
    }

}
