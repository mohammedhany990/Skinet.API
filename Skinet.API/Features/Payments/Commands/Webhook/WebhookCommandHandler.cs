namespace Skinet.API.Features.Payments.Commands.Webhook
{
    /*
    public class WebhookCommandHandler : IRequestHandler<WebhookCommand, BaseResponse<string>>
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<WebhookCommandHandler> _logger;

        public WebhookCommandHandler(IPaymentService paymentService, ILogger<WebhookCommandHandler> logger)
        {
            _paymentService = paymentService;
            _logger = logger;

        }

        public async Task<BaseResponse<string>> Handle(WebhookCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string _endpointSecret = "whsec_ca723e1e0ad297b7c5ab134022aafeb90e12a26c6321e2f191749b4974a15741";

                var stripeEvent = EventUtility.ConstructEvent(request.Json, request.StripeSignature, _endpointSecret, 300, false);
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                if (paymentIntent == null)
                {
                    _logger.LogError("Invalid Stripe Event Data.");
                    return new BaseResponse<string>(400, false, "Invalid Stripe Event Data.");
                }

                switch (stripeEvent.Type)
                {
                    case EventTypes.PaymentIntentSucceeded:
                        await _paymentService.UpdateOrderStatusAsync(paymentIntent.Id, true);
                        _logger.LogInformation("Order succeeded: {0}", paymentIntent.Id);
                        break;

                    case EventTypes.PaymentIntentPaymentFailed:
                        await _paymentService.UpdateOrderStatusAsync(paymentIntent.Id, false);
                        _logger.LogInformation("Order failed: {0}", paymentIntent.Id);
                        break;

                    default:
                        _logger.LogWarning("Unhandled event type: {0}", stripeEvent.Type);
                        return new BaseResponse<string>(400, false, "Unhandled event type.");
                }

                return new BaseResponse<string>(200, true, "Webhook Processed Successfully.");
            }
            catch (StripeException ex)
            {
                _logger.LogError("Stripe error: {0}", ex.Message);
                return new BaseResponse<string>(400, false, "Stripe error occurred.");
            }
        }
    }

    */

}
