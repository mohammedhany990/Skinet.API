using Skinet.Core.Interfaces;
using Skinet.Repository.Abstracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet.Repository
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
