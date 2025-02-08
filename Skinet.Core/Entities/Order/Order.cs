using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet.Core.Entities.Order
{
    public class Order:BaseEntity
    {
        public Order()
        {
            
        }
        public Order(string buyerEmail,
            UserOrderAddress shippingAddress,
            DeliveryMethod? deliveryMethod,
            IReadOnlyList<OrderItem> orderItems,
            decimal subTotal,
            string paymentIntentId
        )
        {
            BuyerEmail = buyerEmail;
            ShipToAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;

        }


        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public UserOrderAddress ShipToAddress { get; set; }
        public DeliveryMethod? DeliveryMethod { get; set; }
        public IReadOnlyList<OrderItem> OrderItems { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public decimal SubTotal { get; set; }

        public string PaymentIntentId { get; set; } = String.Empty;

        public decimal GetTotal=> SubTotal + DeliveryMethod.Price;
        

    }
}
