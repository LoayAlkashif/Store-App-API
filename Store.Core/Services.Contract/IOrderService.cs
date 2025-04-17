using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Core.Entities.Order;

namespace Store.Core.Services.Contract
{
    public interface IOrderService
    {

       Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethod, OrderAddress shippingAddress);

      Task<IEnumerable<Order>?>  GetOrdersForSpecificUserAsync(string buyerEmail);
      Task<Order?>  GetOrdersByIdForSpecificUserAsync(string buyerEmail, int orderId);
    }
}
