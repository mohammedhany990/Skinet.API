using Skinet.Core.Interfaces;

namespace Skinet.Repository.Abstracts
{
    public interface ICartRepositoryFactory
    {
        ICartRepository Create(IUnitOfWork unitOfWork);
    }
}
