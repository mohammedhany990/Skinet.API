using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Skinet.Core.Entities;
using Skinet.Core.Specifications;

namespace Skinet.Core.Interfaces
{
    public interface IGenericRepository<T>  where T : BaseEntity
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T item);
        void Update(T item);
        void Delete(T item);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);


        Task<List<T>> GetAllWithSpecAsync(ISpecification<T> spec);
        Task<T> GetWithSpecAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);
    }
}
