using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.Errors;
using Store.Repository.Data.Context;

namespace Store.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : ControllerBase
    {
        private readonly StoreDbContext context;

        public BuggyController(StoreDbContext context)
        {
            this.context = context;
        }

        [HttpGet("notFound")]
        public async Task< IActionResult> GetNotFound()
        {

          var brand = await context.Brands.FindAsync(100);

            if(brand is null) return NotFound(new ApiErrorResponse(404, "brand with id : 100 is not found "));

            return Ok();
        }


        [HttpGet("serverError")]
        public async Task<IActionResult> GetServerError()
        {

            var brand = await context.Brands.FindAsync(100);

            var brandToString =  brand.ToString(); // willl throw exception ( Null Refrence Exception)

            return Ok();
        }


        [HttpGet("badRequest")]
        public async Task<IActionResult> GetBadRequest()
        {
            return BadRequest();
        }


        [HttpGet("badRequest/{id}")]
        public async Task<IActionResult> GetBadRequestError(int id) // Validation Error
        {

            return Ok();
        }


        [HttpGet("Unauthorized")]
        
        public async Task<IActionResult> GetUnauthorizedError()
        {
            return Unauthorized(new ApiErrorResponse(401));
        }


    }
}
