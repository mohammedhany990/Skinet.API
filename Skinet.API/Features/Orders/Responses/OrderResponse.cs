namespace Skinet.API.Features.Orders.Responses
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; } = null!;
        public DateTimeOffset OrderDate { get; set; }
        public string Status { get; set; }
        public UserOrderAddressResponse ShippingAddress { get; set; }
        public string DeliveryMethod { get; set; }
        public decimal Price { get; set; }

        public ICollection<OrderItemResponse> Items { get; set; } = new HashSet<OrderItemResponse>();
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public string PaymentIntentId { get; set; }
    }
}
