using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.Errors;
using Store.Core.DTO;
using Store.Core.Entities;
using Store.Core.Repositories.Contract;
using Store.Core.Services.Contract;

namespace Store.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService basketService;

        public BasketController(IBasketService basketService)
        {
            this.basketService = basketService;
        }


        [HttpGet("{id}")]
        public async Task< ActionResult<CustomerBasket>> GetBasket(string? id)
        {
            if (id is null) return BadRequest(new ApiErrorResponse(400, "Invalid id"));
           var basket = await basketService.GetBasketAsync(id);

            if (basket is null) return NotFound(new ApiErrorResponse(404)); 

            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdateCustomer(CustomerBasketDto model)
        {
            if (model is null) return BadRequest(new ApiErrorResponse(400));

            var basket = await basketService.UpdateBasketAsync(model);

            if (basket is null) return BadRequest(new ApiErrorResponse(400));

            return Ok(basket);
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteBasket(string id)
        {
            if(id is null) return BadRequest(new ApiErrorResponse(400));

            var flag = await basketService.DeleteBasketAsync(id);

            if (flag is false) return BadRequest(new ApiErrorResponse(400));

            return NoContent();
        }

    }
}
