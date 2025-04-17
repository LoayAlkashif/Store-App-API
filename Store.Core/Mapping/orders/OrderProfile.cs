using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Store.Core.DTO.Orders;
using Store.Core.Entities.Order;

namespace Store.Core.Mapping.orders
{
    public class OrderProfile : Profile
    {

        public OrderProfile(IConfiguration config)
        {
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, options => options.MapFrom(s =>s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, options => options.MapFrom(s =>s.DeliveryMethod.Cost))
                ;

            CreateMap<OrderAddress, AddressOrderDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(p => p.ProductId, options => options.MapFrom(p => p.Product.ProductId))
                .ForMember(p => p.ProductName, options => options.MapFrom(p => p.Product.ProductName))
                .ForMember(p => p.PictureUrl, options => options.MapFrom(p => $"{config["BASEURL"]}{p.Product.PictureUrl}"))
                ;
        }
    }
}
