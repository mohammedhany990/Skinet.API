﻿namespace Skinet.Core.Specifications
{
    public class OrderWithPaymentIntentSpecification : BaseSpecification<Entities.Order.Order>
    {
        public OrderWithPaymentIntentSpecification(string paymentId)
            : base(O => O.PaymentIntentId == paymentId)
        {

        }

    }
}
