using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Skinet.Core.Entities;
using Skinet.Core.Entities.Identity;
using Skinet.Core.Entities.Order;
using Skinet.Core.Interfaces;
using Skinet.Core.Specifications;

namespace Skinet.Service.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Order?> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, UserOrderAddress shippingAddress)
        {
            
            var basket = await _unitOfWork.BasketRepository.GetBasketAsync(basketId);
            if (basket is null || basket?.Items is null || !basket.Items.Any())
            {
                return null; 
            }
          
            List<OrderItem> items = new List<OrderItem>();

            if (basket?.Items?.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                    var productItemOrdered = new ProductItemOrdered
                    {
                        ProductItemId = product.Id,
                        ProductName = product.Name,
                        PictureUrl = product.PictureUrl
                    };

                    var orderItem = new OrderItem
                    {
                        ItemOrdered = productItemOrdered,
                        Price = product.Price,
                        Quantity = item.Quantity
                    };
                    items.Add(orderItem);
                }
            }

            var subTotal = items.Sum(i => i.Price * i.Quantity);

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            if (deliveryMethod is null)
            {
                return null;
            }

           
            if (!string.IsNullOrEmpty(basket?.PaymentIntentId))
            {
                var spec = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);
                var expiredOrder = await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);
                if (expiredOrder is not null)
                {
                    _unitOfWork.Repository<Order>().Delete(expiredOrder);
                }
            }

           

            var order = new Order(
                buyerEmail: buyerEmail,
                shippingAddress: shippingAddress,
                deliveryMethod: deliveryMethod,
                orderItems: items,
                subTotal: subTotal,
                paymentIntentId: basket.PaymentIntentId
            );

            await _unitOfWork.Repository<Order>().AddAsync(order);

            var result = await _unitOfWork.CompleteAsync();


            return result > 0 ? order : null;
        }

        public async Task<List<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecification(buyerEmail);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return orders;
        }

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            var spec = new OrderSpecification(buyerEmail, id);
            var order = await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);
            return order;
        }

        public async Task<List<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var deliveries = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return deliveries;
        }


        
    }
}
