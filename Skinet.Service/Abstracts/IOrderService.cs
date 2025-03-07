using Skinet.Core.Entities.Order;

namespace Skinet.Core.Interfaces
{
    public interface IOrderService
    {
        Task<List<string>> GetAllOrderStatuses();
        Task<List<DeliveryMethod>> GetDeliveryMethodAsync();

        Task<string> CreateOrderAsync(string userId, string buyerEmail, int deliveryMethodId,
            UserOrderAddress shippingAddress);

        Task<Order?> GetOrderByIdAsync(int orderId, string buyerEmail);


        Task<IEnumerable<Order>> GetOrdersForUserAsync(string buyerEmail);

        Task<string> CancelOrderAsync(int orderId);

        Task<string> UpdateOrderStatusAsync(int orderId, string status);
    }

}
