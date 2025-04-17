using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Core.Entities;

namespace Store.Core.Specification.ProductsSpec
{
    public class ProductSpecifications : BaseSpecifications<Product,int>
    {

        public ProductSpecifications(int id) : base(p => p.Id == id)
        {
            ApplyIncludes();
        }

        public ProductSpecifications(ProductSpecParams productSpecParams) : base(
                p=> 
                (string.IsNullOrEmpty(productSpecParams.Search) || p.Name.ToLower().Contains(productSpecParams.Search))
                &&
                (!productSpecParams.BrandId.HasValue|| productSpecParams.BrandId == p.BrandId)
                &&
                (!productSpecParams.TypeId.HasValue || productSpecParams.TypeId == p.TypeId)
                )
        {
            // name, priceAsc, priceDesc
            if (!string.IsNullOrEmpty(productSpecParams.Sort))
            {
                switch (productSpecParams.Sort)
                {
                    case"priceAsc":
                           AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p=> p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(p=> p.Name);
            }
            ApplyIncludes();

            ApplyPagination((productSpecParams.Limit * (productSpecParams.Page - 1)), productSpecParams.Limit);
        }


        private void ApplyIncludes()
        {
            Includes.Add(p => p.Brand);
            Includes.Add(p => p.Type);
        }
    }
}
