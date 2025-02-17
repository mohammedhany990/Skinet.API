using Skinet.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet.Repository.Abstracts
{
    public interface ICartRepositoryFactory
    {
        ICartRepository Create(IUnitOfWork unitOfWork);
    }
}
