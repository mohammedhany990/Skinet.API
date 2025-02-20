using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skinet.Core.Entities;

namespace Skinet.Repository.Configuration
{
    public class FavoriteItemConfiguration : IEntityTypeConfiguration<FavoriteItem>
    {
        public void Configure(EntityTypeBuilder<FavoriteItem> builder)
        {
            // Table Name
            builder.ToTable("FavoriteItems");

            // Primary Key
            builder.HasKey(f => f.Id);

            // Properties
            builder.Property(f => f.Price)
                .HasColumnType("decimal(18,2)") // Ensures proper decimal precision
                .IsRequired();

            builder.Property(f => f.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()") // Default value at DB level
                .IsRequired();

            builder.Property(f => f.ProductName)
                .HasMaxLength(255) // Prevent excessive string size
                .IsRequired();

            // Relationships
            builder.HasOne(f => f.FavoriteList)
                .WithMany(fl => fl.FavoriteItems)
                .HasForeignKey(f => f.FavoriteListId)
                .OnDelete(DeleteBehavior.Cascade); // If a FavoriteList is deleted, its items are deleted too

            builder.HasOne(f => f.Product)
                .WithMany() // Assuming Product does not reference FavoriteItem
                .HasForeignKey(f => f.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent accidental deletion of products

            // Indexes for optimization
            builder.HasIndex(f => f.FavoriteListId);
            builder.HasIndex(f => f.ProductId);
        }
    }

}
