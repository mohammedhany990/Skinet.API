﻿using Skinet.Core.Entities;
using Skinet.Repository.Interfaces;
using Skinet.Service.Interfaces;

namespace Skinet.Service.Implementation
{
    public class ProductTypeService : IProductTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<ProductType>> GetProductTypesAsync()
        {
            return await _unitOfWork.Repository<ProductType>().GetAllAsync();
        }

        public async Task<ProductType> GetProductTypeByIdAsync(int id)
        {
            return await _unitOfWork.Repository<ProductType>().GetByIdAsync(id);
        }

        public async Task<ProductType> GetProductTypeByNameAsync(string name)
        {
            return await _unitOfWork.Repository<ProductType>()
                .FirstOrDefaultAsync(pt => pt.Name.ToLower() == name.ToLower());
        }


        public async Task<string> AddProductTypeAsync(ProductType productType)
        {
            await _unitOfWork.Repository<ProductType>().AddAsync(productType);
            await _unitOfWork.CompleteAsync();
            return "Added Successfully.";
        }

        public async Task<string> UpdateProductTypeAsync(ProductType productType)
        {
            _unitOfWork.Repository<ProductType>().Update(productType);
            await _unitOfWork.CompleteAsync();
            return "Updated Successfully.";
        }

        public async Task<string> DeleteProductTypeAsync(ProductType productType)
        {
            _unitOfWork.Repository<ProductType>().Delete(productType);
            await _unitOfWork.CompleteAsync();
            return "Deleted Successfully.";
        }
    }
}
