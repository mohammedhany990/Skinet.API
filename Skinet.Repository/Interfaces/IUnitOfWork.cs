using Skinet.Core.Entities;

namespace Skinet.Repository.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        Task<int> CompleteAsync();

        Task CommitTransactionAsync();
        Task BeginTransactionAsync();
        Task RollbackTransactionAsync();

        ICartRepository CartRepository { get; set; }
    }
}
