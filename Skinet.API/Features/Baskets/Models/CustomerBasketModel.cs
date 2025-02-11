using Skinet.Core.Entities.Basket;

namespace Skinet.API.Features.Baskets.Models
{
    public class CustomerBasketModel
    {
        public CustomerBasketModel(string id)
        {
            Id = id;
        }
        public string Id { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal ShippingPrice { get; set; }
    }
}
