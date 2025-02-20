using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skinet.Core.Entities;

namespace Skinet.Repository.Configuration
{
    public class FavoriteListConfiguration : IEntityTypeConfiguration<FavoriteList>
    {
        public void Configure(EntityTypeBuilder<FavoriteList> builder)
        {
            // Table Name
            builder.ToTable("FavoriteLists");

            // Primary Key
            builder.HasKey(fl => fl.Id);

            // Properties
            builder.Property(fl => fl.UserId)
                .HasMaxLength(450) // Matches Identity column length
                .IsRequired();

            builder.Property(fl => fl.CreatedDate)
                .HasDefaultValueSql("GETUTCDATE()")
                .IsRequired();

            builder.Property(fl => fl.LastUpdated)
                .IsRequired(false);

            // Index for quick lookup
            builder.HasIndex(fl => fl.UserId);
        }
    }


}
