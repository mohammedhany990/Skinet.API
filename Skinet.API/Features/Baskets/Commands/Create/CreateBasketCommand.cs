using MediatR;
using Skinet.API.DTOs.Basket;
using Skinet.Core.Entities.Basket;
using Skinet.Core.Helper;
using System.ComponentModel.DataAnnotations;

namespace Skinet.API.Features.Baskets.Commands.Create
{
    public class CreateBasketCommand : IRequest<BaseResponse<string>>
    {
        public CreateBasketCommand(string id)
        {
            Id = id;
        }
        public string Id { get; set; }
        public List<BasketItemModel> Items { get; set; } = new List<BasketItemModel>();
        public string? ClientSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal ShippingPrice { get; set; }
    }
}
