using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet.Core.Entities.Cart
{
    public class CartItem
    {
        public string Id { get; set; } 
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public CartItem(int productId, string productName, decimal price, int quantity)
        {
            Id = Guid.NewGuid().ToString(); 
            ProductId = productId;
            ProductName = productName;
            Price = price;
            Quantity = quantity;
        }
    }




}
