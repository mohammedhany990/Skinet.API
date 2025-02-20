using Skinet.Core.Entities.Cart;
using Skinet.Core.Entities.Order;
namespace Skinet.Service.Interfaces
{
    public interface IPaymentService
    {
        Task<Cart?> CreateOrUpdatePaymentIntentAsync(string cartId);
        Task<Order?> ConfirmPaymentAndUpdateOrderAsync(string paymentIntentId);
        //Task<Order?> UpdateOrderStatusAsync(string paymentIntentId, OrderStatus status);
        // Task<bool> ConfirmPaymentAsync(string paymentIntentId);
    }
}
