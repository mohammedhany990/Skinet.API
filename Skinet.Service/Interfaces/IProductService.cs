using Skinet.Core.Entities;
using Skinet.Core.Specifications;

namespace Skinet.Service.Interfaces
{
    public interface IProductService
    {
        Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecificationParameters? parameters);
        Task<Product> GetProductByIdAsync(int id);
        //Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync();
        //Task<IReadOnlyList<ProductType>> GetProductTypesAsync();
        Task<int> GetProductCountAsync(ProductSpecificationParameters? parameters);
        Task<string> AddProductAsync(Product product);
        Task<string> UpdateProductAsync(Product product);
        Task<string> DeleteProductAsync(Product product);
    }
}
