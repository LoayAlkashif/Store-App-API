using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Core;
using Store.Core.Entities;
using Store.Core.Entities.Order;
using Store.Core.Services.Contract;
using Store.Core.Specification.OrdersSpec;

namespace Store.Service.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IBasketService basketService;
        private readonly IPaymentService paymentService;

        public OrderService(IUnitOfWork unitOfWork, IBasketService basketService, IPaymentService paymentService)
        {
            this.unitOfWork = unitOfWork;
            this.basketService = basketService;
            this.paymentService = paymentService;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, OrderAddress shippingAddress)
        {
            var basket = await basketService.GetBasketAsync(basketId);
            if (basket is null) return null;

            var orderItems = new List<OrderItem>();
            if(basket.Items.Count() > 0)
            {

                foreach (var item in basket.Items)
                {
                    var product = await unitOfWork.Repository<Product, int>().GetAsync(item.Id);
                    var productOrderItem = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);
                    var orderItem = new OrderItem(productOrderItem, product.Price, item.Quantity);

                    orderItems.Add(orderItem);
                }

            }

            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod, int>().GetAsync(deliveryMethodId);

            var subTotal = orderItems.Sum(i => i.Price * i.Quantity);

            //To Do
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {

                var spec = new OrderSpecificationWithPaymentIntentId(basket.PaymentIntentId);
                var existOrder = await unitOfWork.Repository<Order, int>().GetWithSpecAsync(spec);
                if (existOrder != null)
                {
                    unitOfWork.Repository<Order, int>().Delete(existOrder);
                }
            }

            var basketDto = await paymentService.CreateOrUpdateIntentIdAsync(basketId);


            var order = new Order(buyerEmail, shippingAddress, deliveryMethod,orderItems,subTotal,basketDto.PaymentIntentId);

            await  unitOfWork.Repository<Order,int>().AddAsync(order);
            var result = await unitOfWork.CompleteAsync();

            if (result <= 0) return null;

            return order;
        }

        public async Task<Order?> GetOrdersByIdForSpecificUserAsync(string buyerEmail, int orderId)
        {
            var spec= new OrderSpecifications(buyerEmail, orderId);
            var order = await unitOfWork.Repository<Order, int>().GetWithSpecAsync(spec);

            if (order is null) return null;

            return order;
        }

        public async Task<IEnumerable<Order>?> GetOrdersForSpecificUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecifications(buyerEmail);
            var orders =  await unitOfWork.Repository<Order, int>().GetAllWithSpecAsync(spec);

            if (orders is null) return null;

            return orders;
        }
    }
}
