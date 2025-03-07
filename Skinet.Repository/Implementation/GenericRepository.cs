using Microsoft.EntityFrameworkCore;
using Skinet.Core.Entities;
using Skinet.Core.Specifications;
using Skinet.Repository.Data;
using System.Linq.Expressions;
using Skinet.Repository.Interfaces;

namespace Skinet.Repository.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly SkinetDbContext _dbContext;

        public GenericRepository(SkinetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region CRUD Operations

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>()
                .FindAsync(id);
        }

        public async Task AddAsync(T item)
        {
            await _dbContext.Set<T>()
                .AddAsync(item);
        }

        public void Update(T item)
        {
            _dbContext.Set<T>()
                .Update(item);
        }

        public void Delete(T item)
        {
            _dbContext.Set<T>()
                .Remove(item);
        }
        public void DeleteRang(T items)
        {
            _dbContext.Set<T>()
                .RemoveRange(items);
        }

        public async Task<int> DeleteWhereAsync(Expression<Func<T, bool>> predicate)
        {
            var entities = await _dbContext.Set<T>()
                .Where(predicate)
                .ToListAsync();

            if (!entities.Any()) return 0;

            _dbContext.Set<T>()
                .RemoveRange(entities);

            return entities.Count;
        }

        #endregion



        #region Querying

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (filter is not null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(predicate);
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>()
                .AnyAsync(predicate);
        }

        #endregion

        #region Specification Pattern

        public async Task<List<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).AsNoTracking().ToListAsync();
        }

        public async Task<T> GetWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), specification);
        }

        #endregion
    }
}
