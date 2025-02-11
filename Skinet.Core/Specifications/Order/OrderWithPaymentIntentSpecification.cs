namespace Skinet.Core.Specifications.Order
{
    internal class OrderWithPaymentIntentSpecification : BaseSpecification<Entities.Order.Order>
    {
        public OrderWithPaymentIntentSpecification(string paymentId)
            : base(O => O.PaymentIntentId == paymentId)
        {

        }
    }
}
