using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Core.Entities.Order;

namespace Store.Core.Specification.OrdersSpec
{
    public class OrderSpecificationWithPaymentIntentId : BaseSpecifications<Order, int>
    {

        public OrderSpecificationWithPaymentIntentId(string paymentIntentId) : base(o => o.PaymentIntentId == paymentIntentId)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
        }
    }
}
