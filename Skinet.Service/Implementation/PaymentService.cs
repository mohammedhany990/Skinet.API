using Microsoft.Extensions.Configuration;
using Skinet.Core.Entities.Basket;
using Skinet.Core.Entities.Order;
using Skinet.Core.Interfaces;
using Skinet.Core.Specifications;
using Skinet.Repository;
using Skinet.Service.Interfaces;
using Stripe;
using Product = Skinet.Core.Entities.Product;

namespace Skinet.Service.Implementation
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public PaymentService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            // 1. Secret Key
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            // 2. Basket 
            var basket = await _unitOfWork.BasketRepository.GetBasketAsync(basketId);
            if (basket is null)
            {
                return null;
            }
            // Initializing Shipping Price 
            var shippingPrice = 0M;


            if (basket.DeliveryMethodId.HasValue)
            {
                var delivery = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                shippingPrice = delivery.Price;
                basket.ShippingPrice = shippingPrice;
            }

            // 3. Total
            if (basket.Items.Count > 0)
            {
                var productRepository = _unitOfWork.Repository<Product>();

                foreach (var item in basket.Items)
                {
                    var product = await productRepository.GetByIdAsync(item.Id);

                    if (product.Price != item.Price)
                    {
                        item.Price = product.Price;
                    }
                }
            }

            var subTotal = basket.Items.Sum(I => I.Price * I.Quantity);


            // 4. Create Payment Intent
            var Service = new PaymentIntentService();

            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId)) // Create
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(subTotal * 100 + shippingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };
                paymentIntent = await Service.CreateAsync(options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else // Update
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(subTotal * 100 + shippingPrice * 100),
                };
                paymentIntent = await Service.UpdateAsync(basket.PaymentIntentId, options);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }

            await _unitOfWork.BasketRepository.UpdateOrCreateBasketAsync(basket);

            return basket;

        }

        public async Task<Order?> UpdateOrderStatusAsync(string paymentIntentId, bool isPaid)
        {
            var spec = new OrderWithPaymentIntentSpecification(paymentIntentId);

            var order = await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);

            if (order is null)
            {
                return null;
            }
            order.Status = isPaid ? OrderStatus.PaymentReceived
                : OrderStatus.PaymentFailed;


            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.CompleteAsync();

            return order;
        }
    }
}
