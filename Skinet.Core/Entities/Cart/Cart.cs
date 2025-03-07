namespace Skinet.Core.Entities.Cart
{
    public class Cart
    {
        public Cart() { }

        public Cart(string userId)
        {
            Id = Guid.NewGuid().ToString();
            UserId = userId;
        }

        public string Id { get; set; }
        public string UserId { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal Total { get; private set; }  // Made it private set to ensure consistency
        public int? DeliveryMethodId { get; set; }
        public decimal ShippingPrice { get; set; }

        public string? PaymentIntentId { get; set; } // Stripe PaymentIntent tracking
        public string? ClientSecret { get; set; } // Stripe Client Secret for frontend payment
        public bool IsPaymentConfirmed { get; set; } = false; // Tracks payment status

        public void CalculateTotal()
        {
            Total = Items.Sum(item => item.TotalPrice) + ShippingPrice; // Using TotalPrice for clarity
        }
    }
}