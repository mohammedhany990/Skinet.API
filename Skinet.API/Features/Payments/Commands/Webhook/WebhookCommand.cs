using MediatR;
using Skinet.Core.Helper;

namespace Skinet.API.Features.Payments.Commands.Webhook
{
    public class WebhookCommand : IRequest<BaseResponse<string>>
    {
        public string Json { get; }
        public string StripeSignature { get; }

        public WebhookCommand(string json, string stripeSignature)
        {
            Json = json;
            StripeSignature = stripeSignature;
        }
    }
}
