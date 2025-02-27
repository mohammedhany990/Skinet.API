namespace Skinet.API.Features.Carts.Responses
{
    public class CartResponse
    {
        public CartResponse(string id)
        {
            Id = id;
        }
        public string Id { get; set; }
        public decimal Total { get; set; }


        public List<CartItemResponse> Items { get; set; } = new List<CartItemResponse>();
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal ShippingPrice { get; set; }
        public bool IsPaymentConfirmed { get; set; } = false; // Tracks payment status

    }
}
