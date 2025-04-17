using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Core.DTO.Products;
using Store.Core.Entities;
using Store.Core.Helper;
using Store.Core.Specification.ProductsSpec;

namespace Store.Core.Services.Contract
{
    public interface IProductService
    {

         Task<PaginationResponse<ProductDTO>> GetAllProductsAsync(ProductSpecParams productSpecParams);
         Task<IEnumerable<TypeBrandDto>> GetAllTypesAsync();
         Task<IEnumerable<TypeBrandDto>> GetAllBrandssAsync();

         Task <ProductDTO> GetProductById(int id);

    }
}
