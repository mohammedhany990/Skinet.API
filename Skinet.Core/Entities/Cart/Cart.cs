using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet.Core.Entities.Cart
{
    public class Cart
    {
        public Cart()
        {
            
        }
        public Cart(string cartId, string userId)
        {
            Id = cartId;
            UserId = userId;
        }


        public string Id { get; set; } 
        public string UserId { get; set; } 
        public List<CartItem> Items { get; set; } = new List<CartItem>(); 
        public decimal Total { get; set; }

        
        public void CalculateTotal()
        {
            Total = Items.Sum(item => item.Quantity * item.Price);
        }
    }
}
