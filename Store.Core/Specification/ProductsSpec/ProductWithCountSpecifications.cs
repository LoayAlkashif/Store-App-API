using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Core.Entities;

namespace Store.Core.Specification.ProductsSpec
{
    public class ProductWithCountSpecifications : BaseSpecifications<Product, int>
    {
        public ProductWithCountSpecifications(ProductSpecParams productSpecParams) 
            : base 
            (
               p =>
               (string.IsNullOrEmpty(productSpecParams.Search) || p.Name.ToLower().Contains(productSpecParams.Search))
                &&
                   (!productSpecParams.BrandId.HasValue || productSpecParams.BrandId == p.BrandId)
                   &&
                   (!productSpecParams.TypeId.HasValue || productSpecParams.TypeId == p.TypeId)
             ) { }
        
    }
}
