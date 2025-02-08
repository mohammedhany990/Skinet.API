using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skinet.Core.Entities;

namespace Skinet.Service.Interfaces
{
    public interface IProductTypeService
    {
        Task<IReadOnlyList<ProductType>> GetProductTypesAsync();
        Task<ProductType> GetProductTypeByIdAsync(int id);
        Task<ProductType> GetProductTypeByNameAsync(string name);
        Task<string> AddProductTypeAsync(ProductType productType);
        Task<string> UpdateProductTypeAsync(ProductType productType);
        Task<string> DeleteProductTypeAsync(ProductType id);
    }
}
