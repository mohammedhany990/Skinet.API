using Microsoft.EntityFrameworkCore.Storage;
using Skinet.Core.Entities;
using Skinet.Repository.Abstracts;
using Skinet.Repository.Data;
using StackExchange.Redis;
using System.Collections;
using Skinet.Repository.Interfaces;

namespace Skinet.Repository.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICartRepository CartRepository { get; set; }

        private readonly SkinetDbContext _dbContext;
        private readonly IConnectionMultiplexer _redis;
        private Hashtable repositories = new Hashtable();
        private readonly ICartRepositoryFactory _cartRepositoryFactory;
        private IDbContextTransaction _transaction; // Store the transaction

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

        public async Task BeginTransactionAsync()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            if (_transaction != null)
            {
                await _transaction.DisposeAsync();
            }
            await _dbContext.DisposeAsync();
        }
    }
}
