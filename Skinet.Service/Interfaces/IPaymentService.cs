using Skinet.Core.Entities.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skinet.Core.Entities.Order;

namespace Skinet.Service.Interfaces
{
    public interface IPaymentService
    {
        Task<CustomerBasket?> CreateOrUpdatePaymentIntentAsync(string basketId);
        Task<Order?> UpdateOrderStatusAsync(string paymentIntentId, bool isPaid);
    }
}
