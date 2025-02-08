using System.ComponentModel.DataAnnotations;
using Skinet.API.DTOs.Basket;
using Skinet.Core.Entities.Basket;

namespace Skinet.API.DTOs.Folder
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; }
        public List<BasketItemDto> Items { get; set; }
    }
}
