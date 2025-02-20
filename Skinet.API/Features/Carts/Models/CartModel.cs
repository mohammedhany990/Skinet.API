namespace Skinet.API.Features.Carts.Models
{
    public class CartModel
    {
        public CartModel(string id)
        {
            Id = id;
        }
        public string Id { get; set; }
        public decimal Total { get; set; }


        public List<CartItemModel> Items { get; set; } = new List<CartItemModel>();
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal ShippingPrice { get; set; }
        public bool IsPaymentConfirmed { get; set; } = false; // Tracks payment status

    }
}
