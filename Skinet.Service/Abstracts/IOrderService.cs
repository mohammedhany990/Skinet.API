using Skinet.Core.Entities.Order;

namespace Skinet.Core.Interfaces
{

    public interface IOrderService
    {
        Task<string> CreateOrderAsync(string userId, string buyerEmail, int deliveryMethodId,
            string cartId, UserOrderAddress shippingAddress);

        Task<Order?> GetOrderByIdAsync(int orderId, string buyerEmail);

        Task<IEnumerable<Order>> GetOrdersForUserAsync(string buyerEmail);

        //Task<IEnumerable<Order>> GetAllOrdersAsync();

        Task<bool> CancelOrderAsync(int orderId);

        Task<bool> UpdateOrderStatusAsync(int orderId, string status);
    }

}
