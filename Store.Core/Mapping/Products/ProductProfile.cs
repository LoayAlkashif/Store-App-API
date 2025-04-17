using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Core.Entities;
using AutoMapper;
using Store.Core.DTO.Products;
using Microsoft.Extensions.Configuration;

namespace Store.Core.Mapping.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile(IConfiguration config)
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(d => d.BrandName, options => options.MapFrom(s => s.Brand.Name))
                .ForMember(d => d.TypeName, options => options.MapFrom(s => s.Type.Name))
                .ForMember(d => d.PictureUrl, options => options.MapFrom(s => $"{config["BASEURL"]}{s.PictureUrl}"));

            CreateMap<ProductBrand,TypeBrandDto>();
            CreateMap<ProductType, TypeBrandDto>();

        }


    }
}
