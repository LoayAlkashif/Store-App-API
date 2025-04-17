using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Core.DTO;
using Store.Core.Entities.Order;

namespace Store.Core.Services.Contract
{
    public interface IPaymentService
    {

       Task<CustomerBasketDto> CreateOrUpdateIntentIdAsync(string baskeId);

       Task<Order> UpdatePaymentIntentForSucceededorFailed(string paymentIntentId, bool flag);

    }
}
