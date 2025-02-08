using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skinet.Core.Entities;
using Skinet.Core.Interfaces;
using Skinet.Service.Interfaces;

namespace Skinet.Service.Implementation
{
    public class ProductBrandService : IProductBrandService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductBrandService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            return await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
        }

        public async Task<ProductBrand> GetProductBrandByIdAsync(int id)
        {
            return await _unitOfWork.Repository<ProductBrand>().GetByIdAsync(id);
        }

        public async Task<ProductBrand> GetProductBrandByNameAsync(string name)
        {
            return await _unitOfWork.Repository<ProductBrand>()
                .FirstOrDefaultAsync(pt => pt.Name.ToLower() == name.ToLower());
        }

        public async Task<string> AddProductBrandAsync(ProductBrand productBrand)
        {
            await _unitOfWork.Repository<ProductBrand>().AddAsync(productBrand);
            await _unitOfWork.CompleteAsync();
            return "Added Successfully.";
        }

        public async Task<string> UpdateProductBrandAsync(ProductBrand productBrand)
        {
            _unitOfWork.Repository<ProductBrand>().Update(productBrand);
            await _unitOfWork.CompleteAsync();
            return "Updated Successfully.";
        }

        public async Task<string> DeleteProductBrandAsync(ProductBrand productBrand)
        {
            _unitOfWork.Repository<ProductBrand>().Delete(productBrand);
            await _unitOfWork.CompleteAsync();
            return "Deleted Successfully.";
        }
    }
}
