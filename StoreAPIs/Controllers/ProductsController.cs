using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.Attributes;
using Store.APIs.Errors;
using Store.Core.DTO.Products;
using Store.Core.Helper;
using Store.Core.Services.Contract;
using Store.Core.Specification.ProductsSpec;

namespace Store.APIs.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }


        [ProducesResponseType(typeof(PaginationResponse<ProductDTO>),StatusCodes.Status200OK)] // a7san 4akl el swager
        [HttpGet]
        [Cached(100)]
        [Authorize]
        // sort => name, asc/desc price
        public async Task<ActionResult<PaginationResponse<ProductDTO>>> GetAllProducts([FromQuery] ProductSpecParams productSpecParams)
        {
            var result = await productService.GetAllProductsAsync(productSpecParams);

            return Ok(result);
        }

        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)] // a7san 4akl el swager
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)] // a7san 4akl el swager
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)] // a7san 4akl el swager
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int? id)
        {
            if (id is null) return BadRequest(new ApiErrorResponse(400, "Invalid id"));
            var result = await productService.GetProductById(id.Value);
            if (result == null) return NotFound(new ApiErrorResponse(404, $"Product with Id: {id} Not Found"));
            return Ok(result);
        }


        [ProducesResponseType(typeof(IEnumerable<TypeBrandDto>), StatusCodes.Status200OK)]
        [HttpGet("Brands")]
        public async Task<IActionResult> GetAllBrands()
        {
            var result = await productService.GetAllBrandssAsync();
            return Ok(result);
        }

        [ProducesResponseType(typeof(IEnumerable<TypeBrandDto>), StatusCodes.Status200OK)]
        [HttpGet("Types")]
        public async Task<IActionResult> GetAllTypes()
        {
            var result = await productService.GetAllTypesAsync();
            return Ok(result);
        }

      

    }
}
