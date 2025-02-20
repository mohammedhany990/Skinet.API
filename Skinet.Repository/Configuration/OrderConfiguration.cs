using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Skinet.Core.Entities.Order;

namespace Skinet.Repository.Configuration
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShipToAddress, a =>
            {
                a.WithOwner();
            });

            builder.Property(s => s.Status)
                .HasConversion(o => o.ToString(),
                    o => (OrderStatus)Enum.Parse(typeof(OrderStatus), o));


            builder.HasMany(o => o.OrderItems)
                .WithOne().OnDelete(DeleteBehavior.Cascade);



            builder.HasOne(o => o.DeliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(t => t.SubTotal)
                .HasColumnType("decimal(18,2)");



        }
    }
}
