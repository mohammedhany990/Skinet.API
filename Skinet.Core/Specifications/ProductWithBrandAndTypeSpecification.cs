using Skinet.Core.Entities;

namespace Skinet.Core.Specifications
{
    public class ProductWithBrandAndTypeSpecification : BaseSpecification<Product>
    {
        public ProductWithBrandAndTypeSpecification(ProductSpecificationParameters? parameters)
            : base(p =>
                (!parameters.BrandId.HasValue || p.ProductBrandId == parameters.BrandId) &&
                (!parameters.TypeId.HasValue || p.ProductTypeId == parameters.TypeId) &&
                (string.IsNullOrEmpty(parameters.search) || p.Name.ToLower().Contains(parameters.search.ToLower())) &&
                (string.IsNullOrEmpty(parameters.BrandName) || p.ProductBrand.Name.ToLower().Contains(parameters.BrandName.ToLower())) &&
                (string.IsNullOrEmpty(parameters.TypeName) || p.ProductType.Name.ToLower().Contains(parameters.TypeName.ToLower()))


                )
        {
            if (!string.IsNullOrEmpty(parameters.sort))
            {
                switch (parameters.sort)
                {
                    case "priceAsc":
                        ApplyOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        ApplyOrderByDesc(p => p.Price);
                        break;


                    case "nameDesc":
                        ApplyOrderByDesc(p => p.Name);
                        break;
                }
            }
            else
            {
                ApplyOrderBy(p => p.Name);
            }
            Includes.Add(p => p.ProductBrand);
            Includes.Add(p => p.ProductType);

            ApplyPagination(parameters.PageSize * (parameters.PageIndex - 1), parameters.PageSize);
        }

        public ProductWithBrandAndTypeSpecification(int id)
        : base(p => p.Id == id)
        {
            Includes.Add(p => p.ProductBrand);
            Includes.Add(p => p.ProductType);
        }
    }
}
