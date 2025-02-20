using Skinet.Core.Entities;
using Skinet.Core.Interfaces;
using Skinet.Repository.Abstracts;
using Skinet.Repository.Data;
using StackExchange.Redis;
using System.Collections;

namespace Skinet.Repository.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICartRepository CartRepository { get; set; }

        private readonly SkinetDbContext _dbContext;
        private readonly IConnectionMultiplexer _redis;
        private Hashtable repositories = new Hashtable();
        private readonly ICartRepositoryFactory _cartRepositoryFactory;

        public UnitOfWork(SkinetDbContext dbContext, IConnectionMultiplexer redis, ICartRepositoryFactory cartRepositoryFactory)
        {
            _dbContext = dbContext;
            _redis = redis;
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
