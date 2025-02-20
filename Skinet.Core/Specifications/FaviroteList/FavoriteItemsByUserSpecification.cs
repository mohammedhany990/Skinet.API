using Skinet.Core.Entities;

namespace Skinet.Core.Specifications.FaviroteList
{
    public class FavoriteItemsByUserSpecification : BaseSpecification<FavoriteItem>
    {
        public FavoriteItemsByUserSpecification(string userId)
            : base(i => i.FavoriteList != null && i.FavoriteList.UserId == userId)
        {
            Includes.Add(f => f.Product);
            Includes.Add(f => f.Product.ProductBrand);
            Includes.Add(f => f.Product.ProductType);
            ApplyOrderByDesc(o => o.CreatedAt);
        }
    }
}
