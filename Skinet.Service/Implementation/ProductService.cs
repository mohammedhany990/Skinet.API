using Skinet.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skinet.Core.Entities;
using Skinet.Core.Interfaces;
using Skinet.Core.Specifications;

namespace Skinet.Service.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Product>> GetProductsAsync(ProductSpecificationParameters? parameters)
        {
            var products = await _unitOfWork.Repository<Product>()
                .GetAllWithSpecAsync(new ProductWithBrandAndTypeSpecification(parameters));
            return products;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Product>()
                .GetWithSpecAsync(new ProductWithBrandAndTypeSpecification(id));
        }

       

        public async Task<int> GetProductCountAsync(ProductSpecificationParameters? parameters)
        {
            return await _unitOfWork.Repository<Product>()
                .CountAsync(new ProductWithCountSpecification(parameters));
        }

        public async Task<string> AddProductAsync(Product product)
        {
            await _unitOfWork.Repository<Product>().AddAsync(product);
            await _unitOfWork.CompleteAsync();
            return "Added Successfully.";
        }

        public async Task<string> UpdateProductAsync(Product product)
        {
            _unitOfWork.Repository<Product>().Update(product);
            await _unitOfWork.CompleteAsync();
            return "Updated Successfully.";
        }

        public async Task<string> DeleteProductAsync(Product product)
        {
            _unitOfWork.Repository<Product>().Delete(product);
            await _unitOfWork.CompleteAsync();
            return "Deleted Successfully.";
        }
    }
}
