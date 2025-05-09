﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using StackExchange.Redis;
using Store.Core.Entities;
using Store.Core.Repositories.Contract;

namespace Store.Repository.Repositories
{
    public class BasketRepository : IBasketRepository
    {

        private readonly IDatabase database;

        public BasketRepository(IConnectionMultiplexer redis)
        {
           database =  redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string basketId)
        {
          return await  database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket>? GetBasketAsync(string basketId)
        {
           var basket =  await database.StringGetAsync(basketId);


            return basket.IsNullOrEmpty ? null: JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket>? UpdateBasketAsync(CustomerBasket basket)
        {
            var createdOrUpdatedBasket = database.StringSet(basket.Id, JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));
            if (createdOrUpdatedBasket is false) return null;

            return await GetBasketAsync(basket.Id);
        }
    }
}
