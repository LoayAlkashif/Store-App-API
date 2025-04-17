using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Core.Entities.Order;
using Stripe;
using Microsoft.Extensions.Configuration;
using Store.Core;
using Store.Core.DTO;
using Store.Core.Services.Contract;
using Product = Store.Core.Entities.Product;
using Store.Core.Specification.OrdersSpec;
using Order = Store.Core.Entities.Order.Order;




namespace Store.Service.Services.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketService basketService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration config;

        public PaymentService(IBasketService basketService, IUnitOfWork unitOfWork, IConfiguration config)
        {
            this.basketService = basketService;
            this.unitOfWork = unitOfWork;
            this.config = config;
        }
        public async Task<CustomerBasketDto> CreateOrUpdateIntentIdAsync(string baskeId)
        {

            StripeConfiguration.ApiKey = config["Stripe:SecretKey"];


            //get basket
            var basket = await basketService.GetBasketAsync(baskeId);
            if (basket is null) return null;

            var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await unitOfWork.Repository<DeliveryMethod, int>().GetAsync(basket.DeliveryMethodId.Value);
                if (deliveryMethod == null) return null;

                shippingPrice = deliveryMethod.Cost;
            }


            if (basket.Items.Count() > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await unitOfWork.Repository<Product, int>().GetAsync(item.Id);
                    if(product is null) return null;
                    if (item.Price != product.Price)
                    {
                        item.Price = product.Price;
                    }
                }
            }

            //if (basket.Items != null && basket.Items.Any())
            //{
            //    foreach (var item in basket.Items)
            //    {
            //        var product = await unitOfWork.Repository<Product, int>().GetAsync(item.Id);
            //        if (product == null) continue; // or handle missing product

            //        if (item.Price != product.Price)
            //        {
            //            item.Price = product.Price;
            //        }
            //    }
            //}

            var subTotal = basket.Items?.Sum(i => i.Price * i.Quantity) ?? 0;

            //stripe service
            var stripeService = new PaymentIntentService();


            PaymentIntent paymentIntent;
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                // Create
                var options = new PaymentIntentCreateOptions()
                {
                    Amount =(long) (subTotal * 100 + shippingPrice * 100),
                    PaymentMethodTypes = new List<string>() { "card"},
                    Currency = "usd"
                };
               paymentIntent = await stripeService.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                // update
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(subTotal * 100 + shippingPrice * 100),
                   
                };
                paymentIntent = await stripeService.UpdateAsync(basket.PaymentIntentId,options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }

           basket =  await basketService.UpdateBasketAsync(basket);

            if (basket is null) return null;

            return basket;
        }

        public async Task<Store.Core.Entities.Order.Order> UpdatePaymentIntentForSucceededorFailed(string paymentIntentId, bool flag)
        {
            var spec = new OrderSpecificationWithPaymentIntentId(paymentIntentId);
            var order = await  unitOfWork.Repository<Order, int>().GetWithSpecAsync(spec);

            if(flag)  {
                order.Status = OrderStatus.PaymentReceved;
            }
            else
            {
                order.Status = OrderStatus.PaymentFailed;
            }

            unitOfWork.Repository<Order, int>().Update(order);
            await unitOfWork.CompleteAsync();

            return order;
        }
    }
}
