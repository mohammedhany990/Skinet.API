using Microsoft.Extensions.Configuration;
using Skinet.Core.Entities.Cart;
using Skinet.Core.Entities.Order;
using Skinet.Core.Interfaces;
using Skinet.Core.Specifications;
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

        public async Task<Cart?> CreateOrUpdatePaymentIntentAsync(string cartId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            var cart = await _unitOfWork.CartRepository.GetCartAsync(cartId);

            if (cart is null) return null;

            decimal shippingPrice = 0M;

            if (cart.DeliveryMethodId.HasValue)
            {
                var delivery = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(cart.DeliveryMethodId.Value);
                shippingPrice = delivery.Price;
                cart.ShippingPrice = shippingPrice;
            }

            // If the cart has items, ensure prices are updated
            if (cart.Items.Any())
            {
                var productIds = cart.Items.Select(i => i.ProductId).ToList();// List of all product IDs
                var products = await _unitOfWork.Repository<Product>()
                    .GetAllAsync(p => productIds.Contains(p.Id));

                foreach (var item in cart.Items)
                {
                    var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                    if (product is not null && product.Price != item.Price)
                    {
                        item.Price = product.Price;
                    }
                }
            }



            var subTotal = cart.Items.Sum(item => item.Price * item.Quantity);

            var service = new PaymentIntentService();
            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(cart.PaymentIntentId)) // Create
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(subTotal * 100 + shippingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };
                paymentIntent = await service.CreateAsync(options);
                cart.PaymentIntentId = paymentIntent.Id;
                cart.ClientSecret = paymentIntent.ClientSecret;
            }
            else // Update
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)(subTotal * 100 + shippingPrice * 100),
                };
                paymentIntent = await service.UpdateAsync(cart.PaymentIntentId, options);
                cart.PaymentIntentId = paymentIntent.Id;
                cart.ClientSecret = paymentIntent.ClientSecret;
            }

            await _unitOfWork.CartRepository.UpdateCartAsync(cart.UserId, cart);

            return cart;
        }


        public async Task<Order?> ConfirmPaymentAndUpdateOrderAsync(string paymentIntentId)
        {
            bool paymentSucceeded = await ConfirmPaymentAsync(paymentIntentId);
            OrderStatus status = paymentSucceeded ? OrderStatus.PaymentReceived : OrderStatus.PaymentFailed;

            return await UpdateOrderStatusAsync(paymentIntentId, status);
        }

        private async Task<Order?> UpdateOrderStatusAsync(string paymentIntentId, OrderStatus status)
        {
            var spec = new OrderWithPaymentIntentSpecification(paymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);

            if (order is null) return null;

            order.Status = status;
            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.CompleteAsync();

            return order;
        }

        private async Task<bool> ConfirmPaymentAsync(string paymentIntentId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            var service = new PaymentIntentService();
            try
            {
                var paymentIntent = await service.GetAsync(paymentIntentId);
                return paymentIntent.Status == "succeeded";
            }
            catch (StripeException)
            {
                return false;
            }
        }
    }
}
