using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.Errors;
using Store.Core;
using Store.Core.DTO.Orders;
using Store.Core.Entities.Order;
using Store.Core.Services.Contract;

namespace Store.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public OrderController(IOrderService orderService, IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.orderService = orderService;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder(OrderDto model)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null) return Unauthorized(new ApiErrorResponse(401));

            var address =  mapper.Map<OrderAddress>(model.ShipToAddress);
            var order = await orderService.CreateOrderAsync(userEmail, model.BasketId, model.DeliveryMethodId,address);

            if (order is null) return BadRequest(new ApiErrorResponse(400));

            return Ok(mapper.Map<OrderToReturnDto>(order));
        }



        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOrderForSpecificUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null) return Unauthorized(new ApiErrorResponse(401));

            var orders = await orderService.GetOrdersForSpecificUserAsync(userEmail);

            if (orders is null) return BadRequest(new ApiErrorResponse(400));

            return Ok(mapper.Map<IEnumerable<OrderToReturnDto>>(orders));
        }


        [HttpGet("{orderId}")]
        [Authorize]
        public async Task<IActionResult> GetOrdersForSpecificUser(int orderId )
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail is null) return Unauthorized(new ApiErrorResponse(401));

            var order = await orderService.GetOrdersByIdForSpecificUserAsync(userEmail, orderId);

            if (order is null) return NotFound(new ApiErrorResponse(404));

            return Ok(mapper.Map<OrderToReturnDto>(order));
        }


        [HttpGet("DeliveryMethods")]
        public async Task<IActionResult> GetAllDeliveryMethods()
        {
            var deliveriesMethod = await unitOfWork.Repository<DeliveryMethod, int>().GetAllAsync();
            if (deliveriesMethod is null) return BadRequest(new ApiErrorResponse(400));

            return Ok(deliveriesMethod);
        }


    }


}
