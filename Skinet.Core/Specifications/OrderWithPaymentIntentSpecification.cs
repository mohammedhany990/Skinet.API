using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Skinet.Core.Entities.Order;

namespace Skinet.Core.Specifications
{
    public class OrderWithPaymentIntentSpecification : BaseSpecification<Order>
    {
        public OrderWithPaymentIntentSpecification(string paymentId)
            : base(O => O.PaymentIntentId == paymentId)
        {

        }

    }
}
