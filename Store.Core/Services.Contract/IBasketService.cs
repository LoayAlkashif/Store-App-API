using System;
using System.Collections.Generic;
using System.Linq;
using Store.Core.DTO;


namespace Store.Core.Services.Contract
{
    public interface IBasketService
    {

        Task<CustomerBasketDto>? GetBasketAsync(string basketId);
        Task<CustomerBasketDto>? UpdateBasketAsync(CustomerBasketDto basket);
        Task<bool> DeleteBasketAsync(string basketId);

    }
}
