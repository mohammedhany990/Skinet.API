using Skinet.Core.Entities.Cart;
using Skinet.Core.Entities.Order;
using Skinet.Core.Interfaces;
using Skinet.Core.Specifications;
using Skinet.Service.Interfaces;
using Stripe;
using Product = Skinet.Core.Entities.Product;

namespace Skinet.Service.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        public OrderService(IUnitOfWork unitOfWork, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
        }

        #region Create Orders

        public async Task<string> CreateOrderAsync(
            string userId,
            string buyerEmail,
            int deliveryMethodId,
            string cartId,
            UserOrderAddress shippingAddress)
        {
            var cart = await _unitOfWork.CartRepository.GetCartAsync(userId);

            if (cart is null || cart.Items is null || !cart.Items.Any())
            {
                return "Cart not found.";
            }

            var items = await CreateOrderItemsAsync(cart.Items);

            var subTotal = items.Sum(i => i.Price * i.Quantity);

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            if (deliveryMethod is null)
            {
                return "Delivery Method not found.";
            }

            await HandlePaymentIntentAsync(cartId, cart);

            var order = new Order(
                buyerEmail: buyerEmail,
                shippingAddress: shippingAddress,
                deliveryMethod: deliveryMethod,
                orderItems: items,
                subTotal: subTotal,
                paymentIntentId: cart.PaymentIntentId?.ToString() ?? string.Empty
            );

            await _unitOfWork.Repository<Order>().AddAsync(order);
            var result = await _unitOfWork.CompleteAsync();

            return result <= 0 ? "Order hasn't been added." : "Order added successfully.";
        }

        private async Task<List<OrderItem>> CreateOrderItemsAsync(IEnumerable<CartItem> cartItems)
        {
            var items = new List<OrderItem>();
            foreach (var item in cartItems)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);
                if (product == null)
                {
                    continue; // Skip if product is not found
                }

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
            return items;
        }

        private async Task HandlePaymentIntentAsync(string cartId, Cart cart)
        {
            if (!string.IsNullOrEmpty(cart?.PaymentIntentId))
            {
                var spec = new OrderWithPaymentIntentSpecification(cart.PaymentIntentId);
                var expiredOrder = await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);
                if (expiredOrder != null)
                {
                    _unitOfWork.Repository<Order>().Delete(expiredOrder);
                    await _paymentService.CreateOrUpdatePaymentIntentAsync(cartId);
                }
            }
        }


        #endregion


        public async Task<Order?> GetOrderByIdAsync(int orderId, string buyerEmail)
        {
            var spec = new OrderSpecification(buyerEmail, orderId);
            var order = await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);
            return order;
        }

        public async Task<IEnumerable<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecification(buyerEmail);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return orders;
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(orderId);
            if (order is null || order.Status == OrderStatus.Cancelled || order.Status == OrderStatus.Shipped)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(order.PaymentIntentId))
            {
                var paymentIntentService = new PaymentIntentService();
                try
                {
                    var paymentIntent = await paymentIntentService.CancelAsync(order.PaymentIntentId);
                    if (paymentIntent.Status != "canceled")
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }

            order.Status = OrderStatus.Cancelled;
            _unitOfWork.Repository<Order>().Update(order);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0;
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(orderId);
            if (order is null || !IsValidStatusTransition(order.Status, status))
            {
                return false;
            }

            order.Status = GetOrderStatus(status);
            _unitOfWork.Repository<Order>().Update(order);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0;
        }

        private bool IsValidStatusTransition(OrderStatus currentStatus, string status)
        {
            return status.ToLower() switch
            {
                "paymentreceived" => currentStatus == OrderStatus.Pending,
                "processing" => currentStatus == OrderStatus.PaymentReceived,
                "shipped" => currentStatus == OrderStatus.Processing,
                "delivered" => currentStatus == OrderStatus.Shipped,
                "paymentfailed" => currentStatus == OrderStatus.Pending,
                _ => false
            };
        }

        private OrderStatus GetOrderStatus(string status)
        {
            return status.ToLower() switch
            {
                "paymentreceived" => OrderStatus.PaymentReceived,
                "processing" => OrderStatus.Processing,
                "shipped" => OrderStatus.Shipped,
                "delivered" => OrderStatus.Delivered,
                "paymentfailed" => OrderStatus.PaymentFailed,
                _ => OrderStatus.Pending
            };
        }
    }
}
