using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Skinet.API.Features.Baskets.Models;
using Skinet.API.Features.Payments.Commands.Update;
using Skinet.API.Features.Payments.Commands.Webhook;

namespace Skinet.API.Controllers
{
    [ApiVersion("1.0")]
    public class PaymentsController : ApiBaseController
    {
        private readonly IMediator _mediator;
       
        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
          
        }

        [HttpPost("{basketId}")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<CustomerBasketModel>> CreateOrUpdatePayment(UpdatePaymentsCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("Webhook")]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeSignature = Request.Headers["Stripe-Signature"];

            var response = await _mediator.Send(new WebhookCommand(json, stripeSignature));

            return StatusCode(response.StatusCode, response);
        }




    }
}
