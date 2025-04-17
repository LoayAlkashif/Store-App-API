using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Core.DTO.Orders
{
    public class OrderDto
    {

        public string BasketId { get; set; }
        public int DeliveryMethodId { get; set; }

        public AddressOrderDto ShipToAddress { get; set; }
    }
}
