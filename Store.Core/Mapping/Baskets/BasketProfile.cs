using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Store.Core.DTO;
using Store.Core.Entities;

namespace Store.Core.Mapping.Baskets
{ 
    public class BasketProfile : Profile
    {

        public BasketProfile()
        {
            CreateMap<CustomerBasketDto, CustomerBasket>().ReverseMap();
            CreateMap<BasketItemDto, BasketItem>().ReverseMap();
        }
    }
}
