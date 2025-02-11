using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet.Core.Specifications.Order
{
    public class OrderSpecification : BaseSpecification<Entities.Order.Order>
    {
        public OrderSpecification(string email)
            : base(O => O.BuyerEmail == email)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.OrderItems);
            ApplyOrderBy(O => O.OrderDate);
        }

        public OrderSpecification(string buyerEmail, int orderId)
            : base(O => O.BuyerEmail == buyerEmail && O.Id == orderId)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.OrderItems);
        }
}
}
