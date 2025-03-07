using Microsoft.EntityFrameworkCore;
using Skinet.Core.Entities;
using Skinet.Core.Entities.Order;
using System.Reflection;

namespace Skinet.Repository.Data
{
    public class SkinetDbContext : DbContext
    {
        public SkinetDbContext(DbContextOptions<SkinetDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }
        public DbSet<FavoriteItem> FavoriteItems { get; set; }
        public DbSet<FavoriteList> FavoriteLists { get; set; }
    }
}
