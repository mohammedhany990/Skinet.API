using Skinet.Core.Entities;

namespace Skinet.Service.Interfaces
{
    public interface IProductBrandService
    {
        Task<List<ProductBrand>> GetProductBrandsAsync();
        Task<ProductBrand> GetProductBrandByIdAsync(int id);
        Task<ProductBrand> GetProductBrandByNameAsync(string name);

        Task<string> AddProductBrandAsync(ProductBrand productBrand);
        Task<string> UpdateProductBrandAsync(ProductBrand productBrand);
        Task<string> DeleteProductBrandAsync(ProductBrand productBrand);

    }
}
