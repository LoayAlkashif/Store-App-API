using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Store.Core;
using Store.Core.DTO.Products;
using Store.Core.Entities;
using Store.Core.Helper;
using Store.Core.Services.Contract;
using Store.Core.Specification;
using Store.Core.Specification.ProductsSpec;

namespace Store.Service.Services.Products
{
    public class ProductService : IProductService
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<PaginationResponse<ProductDTO>> GetAllProductsAsync(ProductSpecParams productSpecParams)
        {
            var spec = new ProductSpecifications(productSpecParams);

            var products = await unitOfWork.Repository<Product, int>().GetAllWithSpecAsync(spec);
            var mappedProducts = mapper.Map<IEnumerable<ProductDTO>>(products);

            var countSpec = new ProductWithCountSpecifications(productSpecParams);

            var count = await unitOfWork.Repository<Product, int>().GetCountAsync(countSpec);

            return new PaginationResponse<ProductDTO>(productSpecParams.Limit, productSpecParams.Page, count, mappedProducts);
        }


        public async Task<ProductDTO> GetProductById(int id)
        {
            var spec = new ProductSpecifications(id);
            var product = await unitOfWork.Repository<Product, int>().GetWithSpecAsync(spec);
            var mappedProduct = mapper.Map<ProductDTO>(product);

            return mappedProduct;
        }

        public async Task<IEnumerable<TypeBrandDto>> GetAllTypesAsync()
        {
            return mapper.Map<IEnumerable<TypeBrandDto>>(await unitOfWork.Repository<ProductType, int>().GetAllAsync());

        }

        public async Task<IEnumerable<TypeBrandDto>> GetAllBrandssAsync()
        {
            var brands = await unitOfWork.Repository<ProductBrand, int>().GetAllAsync();
            var mappedBrands = mapper.Map<IEnumerable<TypeBrandDto>>(brands);

            return mappedBrands;

        }

    }
}


