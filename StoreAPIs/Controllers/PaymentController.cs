using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.Errors;
using Store.Core.Services.Contract;
using Stripe;
using Stripe.Events;

namespace Store.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        [HttpPost("{basketId}")]
        [Authorize]
        public async Task< IActionResult> CreatePayment(string basketId)
        {
            if (basketId is null) return BadRequest(new ApiErrorResponse(400));
            var basket = await paymentService.CreateOrUpdateIntentIdAsync(basketId);
            if (basket is null) return BadRequest(new ApiErrorResponse(400));

            return Ok(basket);
        }


        // This is your Stripe CLI webhook secret for testing your endpoint locally.

        [HttpPost("webhook")]//https://localhost:44393//api/payments/webhook
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            const string endpointSecret = "";

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);

                var paymentIntentId = stripeEvent.Data.Object as PaymentIntent;

                // Handle the event
                if (stripeEvent.Type == "payment_intent.payment_failed")
                {
                    // update data base
                    await paymentService.UpdatePaymentIntentForSucceededorFailed(paymentIntentId.Id, false);
                }
                // ... handle other event types

                else if(stripeEvent.Type == "payment_intent.succeeded")
                {
                    // update database
                    await paymentService.UpdatePaymentIntentForSucceededorFailed(paymentIntentId.Id, true);

                }
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }
    }
}
