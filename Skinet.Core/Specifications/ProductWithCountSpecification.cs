using Skinet.Core.Entities;

namespace Skinet.Core.Specifications
{
    public class ProductWithCountSpecification  : BaseSpecification<Product>
    {
        public ProductWithCountSpecification(ProductSpecificationParameters? parameters)
            : base(p =>
                (!parameters.BrandId.HasValue || p.ProductBrandId == parameters.BrandId) &&
                (!parameters.TypeId.HasValue || p.ProductTypeId == parameters.TypeId) &&
                (string.IsNullOrEmpty(parameters.search) || p.Name.ToLower().Contains(parameters.search.ToLower())) &&
                (string.IsNullOrEmpty(parameters.BrandName) ||
                 p.ProductBrand.Name.ToLower().Contains(parameters.BrandName.ToLower())) &&
                (string.IsNullOrEmpty(parameters.TypeName) ||
                 p.ProductType.Name.ToLower().Contains(parameters.TypeName.ToLower()))


            )
        {

        }
    }
}
