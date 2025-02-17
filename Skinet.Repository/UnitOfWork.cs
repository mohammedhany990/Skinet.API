using Skinet.Core.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skinet.Core.Entities;
using Skinet.Repository.Data;
using StackExchange.Redis;
using Skinet.Repository.Abstracts;

namespace Skinet.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public IBasketRepository BasketRepository { get; set; }
        public ICartRepository CartRepository { get; set; }

        private readonly SkinetDbContext _dbContext;
        private readonly IConnectionMultiplexer _redis;
        private Hashtable repositories = new Hashtable();
        private readonly ICartRepositoryFactory _cartRepositoryFactory;

        public UnitOfWork(SkinetDbContext dbContext, IConnectionMultiplexer redis, ICartRepositoryFactory cartRepositoryFactory)
        {
            _dbContext = dbContext;
            _redis = redis;
            BasketRepository = new BasketRepository(_redis);
            _cartRepositoryFactory = cartRepositoryFactory;
            CartRepository = _cartRepositoryFactory.Create(this);
           
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var key = typeof(TEntity).Name;

            if (!repositories.ContainsKey(key))
            {
                var repository = new GenericRepository<TEntity>(_dbContext);
                repositories.Add(key, repository);
            }

            return (IGenericRepository<TEntity>)repositories[key];
        }



        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }


        public async ValueTask DisposeAsync()
        {
            await _dbContext.DisposeAsync();
        }
    }
}
