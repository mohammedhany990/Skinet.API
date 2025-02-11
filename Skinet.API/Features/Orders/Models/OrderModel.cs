using Skinet.API.DTOs.Order;
using Skinet.Core.Entities.Order;

namespace Skinet.API.Features.Orders.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; } = null!;
        public DateTimeOffset OrderDate { get; set; }
        public string Status { get; set; }
        public UserOrderAddressModel ShippingAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal Price { get; set; }

        public ICollection<OrderItemModel> Items { get; set; } = new HashSet<OrderItemModel>();
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public string PaymentIntentId { get; set; }
    }
}
