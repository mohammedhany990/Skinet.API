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


        // Basic CRUD Operations
        Task<T?> GetByIdAsync(int id);
        //Task<T> GetByKeyAsync(params object[] keyValues); // composite keys
        Task AddAsync(T item);
        void Update(T item);
        void Delete(T item);
        void DeleteRang(T items);
        Task<int> DeleteWhereAsync(Expression<Func<T, bool>> predicate);

      

        // Querying
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);
        Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);


        // Specification Pattern
        Task<List<T>> GetAllWithSpecAsync(ISpecification<T> spec);
        Task<T> GetWithSpecAsync(ISpecification<T> spec);
        Task<int> CountAsync(ISpecification<T> spec);


    }
}
