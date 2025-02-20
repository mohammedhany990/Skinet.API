using Skinet.Core.Entities;

namespace Skinet.Service.Interfaces
{
    public interface IProductTypeService
    {
        Task<List<ProductType>> GetProductTypesAsync();
        Task<ProductType> GetProductTypeByIdAsync(int id);
        Task<ProductType> GetProductTypeByNameAsync(string name);
        Task<string> AddProductTypeAsync(ProductType productType);
        Task<string> UpdateProductTypeAsync(ProductType productType);
        Task<string> DeleteProductTypeAsync(ProductType id);
    }
}
