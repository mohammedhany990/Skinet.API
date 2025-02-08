using Skinet.Core.Entities.Order;

namespace Skinet.API.DTOs.Order
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; } = null!;
        public DateTimeOffset OrderDate { get; set; }
        public string Status { get; set; }
        public UserOrderAddress ShippingAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal Price { get; set; }

        public ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>();
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public string PaymentIntentId { get; set; }
    }
}
