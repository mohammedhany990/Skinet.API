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

        public async Task<Cart?> CreateOrUpdatePaymentIntentAsync(string userId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            var cart = await _unitOfWork.CartRepository.GetCartAsync(userId);
            if (cart is null) return null;

            decimal shippingPrice = 0M;

            if (cart.DeliveryMethodId.HasValue)
            {
                var delivery = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(cart.DeliveryMethodId.Value);
                shippingPrice = delivery?.Price ?? 0M;
                cart.ShippingPrice = shippingPrice;
            }

            // Ensure item prices are updated
            if (cart.Items.Any())
            {
                var productIds = cart.Items.Select(i => i.ProductId).ToList();
                var products = await _unitOfWork.Repository<Product>().GetAllAsync(p => productIds.Contains(p.Id));

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
            
            var totalAmount = (long)((subTotal + shippingPrice) * 100); // Convert to cents
            
            var service = new PaymentIntentService();
            PaymentIntent paymentIntent;

            try
            {
                if (string.IsNullOrEmpty(cart.PaymentIntentId)) // Create new PaymentIntent
                {
                    var options = new PaymentIntentCreateOptions
                    {
                        Amount = totalAmount,
                        Currency = "usd",
                        PaymentMethodTypes = new List<string> { "card" },
                        PaymentMethod = "pm_card_visa", // ❗ Ensure this is a valid PaymentMethod ID
                        Confirm = true // Auto-confirm the payment intent
                    };

                    paymentIntent = await service.CreateAsync(options);
                }
                else
                {
                    paymentIntent = await service.GetAsync(cart.PaymentIntentId);

                    if (paymentIntent.Status == "canceled") // If canceled, create a new one
                    {
                        var options = new PaymentIntentCreateOptions
                        {
                            Amount = totalAmount,
                            Currency = "usd",
                            PaymentMethodTypes = new List<string> { "card" },
                            PaymentMethod = "pm_card_visa", 
                            Confirm = true // Auto-confirm the payment intent
                        };

                        paymentIntent = await service.CreateAsync(options);
                    }
                    else // Otherwise, update existing PaymentIntent
                    {
                        var options = new PaymentIntentUpdateOptions
                        {
                            Amount = totalAmount
                        };
                        paymentIntent = await service.UpdateAsync(cart.PaymentIntentId, options);
                    }
                }

                cart.PaymentIntentId = paymentIntent.Id;
                cart.ClientSecret = paymentIntent.ClientSecret;

                await _unitOfWork.CartRepository.UpdateCartAsync(cart.UserId, cart);
                return cart;
            }
            catch (StripeException ex)
            {
                Console.WriteLine($"Stripe Exception: {ex.Message}");
                return null;
            }
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

            int result = await _unitOfWork.CompleteAsync();

            //Console.WriteLine($"Database update result: {result}");

            return result > 0 ? order : null;
        }

        private async Task<bool> ConfirmPaymentAsync(string paymentIntentId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            var service = new PaymentIntentService();

            try
            {
                var paymentIntent = await service.GetAsync(paymentIntentId);
                Console.WriteLine($"PaymentIntent Status: {paymentIntent.Status}"); // Debug log

                if (paymentIntent.Status == "requires_payment_method")
                {
                    //Console.WriteLine("PaymentIntent is missing a payment method. Ensure the frontend attaches one.");
                    return false;
                }

                if (paymentIntent.Status == "requires_confirmation")
                {
                   
                    await service.ConfirmAsync(paymentIntentId);
                    paymentIntent = await service.GetAsync(paymentIntentId); // Refresh status
                }

                return paymentIntent.Status == "succeeded";
            }
            catch (StripeException ex)
            {
                Console.WriteLine($"StripeException: {ex.Message}");
                return false;
            }
        }

    }
}
