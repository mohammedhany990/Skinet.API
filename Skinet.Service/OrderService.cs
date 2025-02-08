using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skinet.Core.Entities;
using Skinet.Core.Entities.Identity;
using Skinet.Core.Entities.Order;
using Skinet.Core.Interfaces;
using Skinet.Core.Specifications;

namespace Skinet.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IBasketRepository basketRepository,
            IUnitOfWork unitOfWork)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }


        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, UserOrderAddress shippingAddress)
        {
            var basket = await _basketRepository.GetBasketAsync(basketId);

            List<OrderItem> items = new List<OrderItem>();
            
            if(basket?.Items?.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                    var producItemOrdered = new ProductItemOrdered
                    {
                        ProductItemId = product.Id,
                        ProductName = product.Name,
                        PictureUrl = product.PictureUrl
                    };

                    var orderItem = new OrderItem
                    {
                       ItemOrdered = producItemOrdered,
                       Price = product.Price,
                       Quantity = item.Quantity
                    };
                    items.Add(orderItem);
                }
            }

            var subTotal = items.Sum(i => i.Price * i.Quantity);

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            var spec = new OrderWithPaymentIntentSpecification(basket.PaymentIntentId);

            var expiredOrder = await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);

            if (expiredOrder is null)
            {
                _unitOfWork.Repository<Order>().Delete(expiredOrder);
               
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

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
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

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var deliveries = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return deliveries;
        }
    }
}
