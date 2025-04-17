using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Store.Core.DTO;
using Store.Core.Entities;
using Store.Core.Repositories.Contract;
using Store.Core.Services.Contract;

namespace Store.Service.Services.Baskets
{
    public class BasketService : IBasketService
    {
        private readonly IMapper mapper;
        private readonly IBasketRepository basketRepo;

        public BasketService(IBasketRepository basketRepo, IMapper mapper)
        {
            this.basketRepo = basketRepo;
            this.mapper = mapper;
        }

        public async Task<CustomerBasketDto>? GetBasketAsync(string basketId)
        {
            var basket = await basketRepo.GetBasketAsync(basketId);
            if(basket is null) return mapper.Map<CustomerBasketDto>(new CustomerBasket() { Id = basketId });

            return mapper.Map<CustomerBasketDto>(basket);
        }

        public async Task<CustomerBasketDto>? UpdateBasketAsync(CustomerBasketDto basketDto)
        {
            var basket = await basketRepo.UpdateBasketAsync(mapper.Map<CustomerBasket>(basketDto));

            if (basket is null) return null;

            return mapper.Map<CustomerBasketDto>(basket);
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await basketRepo.DeleteBasketAsync(basketId);
        }
    }
}
