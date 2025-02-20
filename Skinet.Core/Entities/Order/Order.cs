namespace Skinet.Core.Entities.Order
{
    public class Order : BaseEntity
    {
        public Order() { }

        public Order(string buyerEmail,
            UserOrderAddress shippingAddress,
            DeliveryMethod deliveryMethod,
            List<OrderItem> orderItems,
            decimal subTotal,
            string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShipToAddress = shippingAddress;
            DeliveryMethod = deliveryMethod ?? throw new ArgumentNullException(nameof(deliveryMethod));
            OrderItems = orderItems ?? new List<OrderItem>();
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId ?? string.Empty;
            Status = OrderStatus.Pending;
        }

        public string BuyerEmail { get; set; } = string.Empty;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public UserOrderAddress ShipToAddress { get; set; } = null!;
        public DeliveryMethod DeliveryMethod { get; set; } = null!;
        public List<OrderItem> OrderItems { get; set; } = new();
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public decimal SubTotal { get; set; }
        public string PaymentIntentId { get; set; } = string.Empty;



        public decimal Total => SubTotal + (DeliveryMethod?.Price ?? 0);
    }
}
