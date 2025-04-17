using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Core.Entities.Order;

namespace Store.Core.Specification.OrdersSpec
{
    public class OrderSpecifications : BaseSpecifications<Order, int>
    {

        public OrderSpecifications(string buyerEmail, int orderId) 
            :base(O=> O.BuyerEmail == buyerEmail && O.Id == orderId)
        {
            ApplyIncludes();
        }

        public OrderSpecifications(string buyerEmail)
         : base(O => O.BuyerEmail == buyerEmail)
        {
            ApplyIncludes();
        }

        private void ApplyIncludes()
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);

        }

    }
}
