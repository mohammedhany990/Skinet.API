using Skinet.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skinet.Core.Entities;
using Skinet.Core.Specifications;
using Skinet.Repository.Data;
using System.Linq.Expressions;

namespace Skinet.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly SkinetDbContext _dbContext;

        public GenericRepository(SkinetDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbContext.Set<T>().FirstOrDefaultAsync(predicate);
        }


        public async Task AddAsync(T item)
        {
            await _dbContext.Set<T>().AddAsync(item);
        }

        public void Update(T item)
        {
            _dbContext.Set<T>().Update(item);
        }

        public void Delete(T item)
        {
            _dbContext.Set<T>().Remove(item);
        }



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
    }
}
