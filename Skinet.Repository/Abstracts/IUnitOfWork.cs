using Skinet.Core.Entities;

namespace Skinet.Core.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        Task<int> CompleteAsync();
       
        ICartRepository CartRepository { get; set; }
    }
}
