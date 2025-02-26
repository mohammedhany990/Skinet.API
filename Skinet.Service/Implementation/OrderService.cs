using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Skinet.Core.Entities;
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
        private readonly IConfiguration _configuration;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IUnitOfWork unitOfWork,
            IPaymentService paymentService,
            IConfiguration configuration,
            ILogger<OrderService> logger)
        {
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<List<string>> GetAllOrderStatuses()
        {
            _logger.LogInformation("Retrieving all order statuses.");
            var statuses = Enum.GetValues(typeof(OrderStatus))
                .Cast<OrderStatus>()
                .Select(s => s.ToString())
                .ToList();

            _logger.LogInformation("Retrieved {Count} order statuses.", statuses.Count);
            return statuses;
        }

        public async Task<List<DeliveryMethod>> GetDeliveryMethodAsync()
        {
            _logger.LogInformation("Retrieving all delivery methods.");
            var deliveries = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            _logger.LogInformation("Retrieved {Count} delivery methods.", deliveries.Count);
            return deliveries;
        }

        public async Task<string> CreateOrderAsync(
            string userId,
            string buyerEmail,
            int deliveryMethodId,
            UserOrderAddress shippingAddress)
        {
            _logger.LogInformation("Starting to create an order for user {UserId}.", userId);
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var cart = await _unitOfWork.CartRepository.GetCartAsync(userId);
                if (cart is null || cart.Items is null || !cart.Items.Any())
                {
                    _logger.LogWarning("Cart not found for user {UserId}.", userId);
                    await _unitOfWork.RollbackTransactionAsync();
                    return "Cart not found.";
                }

                var items = await CreateOrderItemsAsync(cart.Items);
                if (items == null || !items.Any())
                {
                    _logger.LogWarning("Some products are no longer available for user {UserId}.", userId);
                    await _unitOfWork.RollbackTransactionAsync();
                    return "Some products are no longer available.";
                }

                var subTotal = items.Sum(i => i.Price * i.Quantity);

                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
                if (deliveryMethod is null)
                {
                    _logger.LogWarning("Delivery method {DeliveryMethodId} not found.", deliveryMethodId);
                    await _unitOfWork.RollbackTransactionAsync();
                    return "Delivery Method not found.";
                }

                await HandlePaymentIntentAsync(userId, cart);

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

                if (result <= 0)
                {
                    _logger.LogError("Failed to add order for user {UserId}.", userId);
                    await _unitOfWork.RollbackTransactionAsync();
                    return "Order hasn't been added.";
                }

                await _unitOfWork.CommitTransactionAsync();
                _logger.LogInformation("Order created successfully for user {UserId}.", userId);
                return "Order added successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order for user {UserId}.", userId);
                await _unitOfWork.RollbackTransactionAsync();
                return "Error creating order.";
            }
        }

        private async Task<List<OrderItem>> CreateOrderItemsAsync(IEnumerable<CartItem> cartItems)
        {
            _logger.LogInformation("Creating order items from cart items.");
            var items = new List<OrderItem>();
            foreach (var item in cartItems)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.ProductId);
                if (product == null)
                {
                    _logger.LogWarning("Product {ProductId} not found in the database.", item.ProductId);
                    continue;
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

            _logger.LogInformation("Created {Count} order items.", items.Count);
            return items;
        }

        private async Task HandlePaymentIntentAsync(string userId, Cart cart)
        {
            if (string.IsNullOrEmpty(cart?.PaymentIntentId))
            {
                _logger.LogInformation("No payment intent found for user {UserId}.", userId);
                return;
            }

            _logger.LogInformation("Handling payment intent for user {UserId}.", userId);
            var spec = new OrderWithPaymentIntentSpecification(cart.PaymentIntentId);
            var expiredOrder = await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);

            if (expiredOrder != null)
            {
                _logger.LogInformation("Deleting expired order for user {UserId}.", userId);
                _unitOfWork.Repository<Order>().Delete(expiredOrder);
                await _unitOfWork.CompleteAsync();
                await _paymentService.CreateOrUpdatePaymentIntentAsync(userId);
            }
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId, string buyerEmail)
        {
            _logger.LogInformation("Retrieving order {OrderId} for buyer {BuyerEmail}.", orderId, buyerEmail);
            var spec = new OrderSpecification(buyerEmail, orderId);
            var order = await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);

            if (order == null)
            {
                _logger.LogWarning("Order {OrderId} not found for buyer {BuyerEmail}.", orderId, buyerEmail);
            }
            else
            {
                _logger.LogInformation("Order {OrderId} retrieved successfully for buyer {BuyerEmail}.", orderId, buyerEmail);
            }

            return order;
        }

        public async Task<IEnumerable<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            _logger.LogInformation("Retrieving all orders for buyer {BuyerEmail}.", buyerEmail);
            var spec = new OrderSpecification(buyerEmail);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);

            _logger.LogInformation("Retrieved {Count} orders for buyer {BuyerEmail}.", orders.Count(), buyerEmail);
            return orders;
        }

        public async Task<string> CancelOrderAsync(int orderId)
        {
            _logger.LogInformation("Attempting to cancel order {OrderId}.", orderId);
            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(orderId);

            if (order is null)
            {
                _logger.LogWarning("Order {OrderId} not found.", orderId);
                return "Order not found.";
            }

            if (order.Status == OrderStatus.Cancelled)
            {
                _logger.LogWarning("Order {OrderId} is already cancelled.", orderId);
                return "Order is already cancelled.";
            }

            if (order.Status == OrderStatus.Shipped)
            {
                _logger.LogWarning("Order {OrderId} cannot be cancelled because it has already been shipped.", orderId);
                return "Order cannot be cancelled because it has already been shipped.";
            }

            // Cancel the payment intent if it exists
            if (!string.IsNullOrEmpty(order.PaymentIntentId))
            {
                _logger.LogInformation("Cancelling payment intent for order {OrderId}.", orderId);
                StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

                var paymentIntentService = new PaymentIntentService();
                var paymentIntent = await paymentIntentService.CancelAsync(order.PaymentIntentId);

                // Validate the payment intent cancellation
                if (paymentIntent.Status != "canceled")
                {
                    _logger.LogError("Failed to cancel payment intent for order {OrderId}.", orderId);
                    return "Failed to cancel the payment intent.";
                }
            }

            // Update the order status to be Cancelled
            order.Status = OrderStatus.Cancelled;
            _unitOfWork.Repository<Order>().Delete(order);

            var result = await _unitOfWork.CompleteAsync();

            if (result > 0)
            {
                _logger.LogInformation("Order {OrderId} cancelled successfully and deleted.", orderId);
                return "Order cancelled successfully and deleted.";
            }
            else
            {
                _logger.LogError("Failed to cancel order {OrderId}.", orderId);
                return "Failed to cancel the order.";
            }
        }

        public async Task<string> UpdateOrderStatusAsync(int orderId, string status)
        {
            _logger.LogInformation("Attempting to update status of order {OrderId} to {Status}.", orderId, status);

            if (string.IsNullOrWhiteSpace(status))
            {
                _logger.LogWarning("Invalid status provided for order {OrderId}.", orderId);
                return "Invalid Status: Status cannot be null or empty.";
            }

            var order = await _unitOfWork.Repository<Order>().GetByIdAsync(orderId);
            if (order is null)
            {
                _logger.LogWarning("Order {OrderId} not found.", orderId);
                return "Order Not Found.";
            }

            if (!IsValidStatusTransition(order.Status, status))
            {
                _logger.LogWarning("Invalid status transition for order {OrderId}. Current status: {CurrentStatus}, New status: {NewStatus}.",
                    orderId, order.Status, status);
                return "Invalid Status Transition.";
            }

            var newStatus = GetOrderStatus(status);
            if (newStatus is null)
            {
                _logger.LogWarning("Invalid status provided for order {OrderId}: {Status}.", orderId, status);
                return "Invalid Status: The provided status is not recognized.";
            }

            order.Status = newStatus.Value;
            _unitOfWork.Repository<Order>().Update(order);

            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var result = await _unitOfWork.CompleteAsync();

                if (result <= 0)
                {
                    _logger.LogError("Failed to save changes for order {OrderId}.", orderId);
                    await _unitOfWork.RollbackTransactionAsync();
                    return "Save Failed: Unable to save changes to the database.";
                }

                await _unitOfWork.CommitTransactionAsync();
                _logger.LogInformation("Order {OrderId} status updated successfully to {Status}.", orderId, status);
                return "Success: Order status updated successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating status for order {OrderId}.", orderId);
                await _unitOfWork.RollbackTransactionAsync();
                return "Error: An error occurred while updating the order status.";
            }
        }

        private bool IsValidStatusTransition(OrderStatus currentStatus, string status)
        {
            var newStatus = GetOrderStatus(status);
            if (newStatus == null)
            {
                return false;
            }

            return (currentStatus, newStatus.Value) switch
            {
                (OrderStatus.Pending, OrderStatus.PaymentReceived) => true,
                (OrderStatus.PaymentReceived, OrderStatus.Processing) => true,
                (OrderStatus.Processing, OrderStatus.Shipped) => true,
                (OrderStatus.Shipped, OrderStatus.Delivered) => true,
                (OrderStatus.Pending, OrderStatus.PaymentFailed) => true,
                _ => false
            };
        }

        private OrderStatus? GetOrderStatus(string status)
        {
            return Enum.TryParse(typeof(OrderStatus), status, true, out var result)
                ? (OrderStatus)result
                : null;
        }
    }

}
