using Asp.Versioning;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skinet.API.DTOs.Folder;
using Skinet.API.Errors;
using Stripe;
using Skinet.Core.Entities.Basket;
using Skinet.Service.Interfaces;
using Skinet.Core.Entities.Order;
using Skinet.API.Features.Baskets.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Skinet.API.Features.Payments.Commands.Update;
using Skinet.API.Features.Payments.Commands.Webhook;

namespace Skinet.API.Controllers
{
    [ApiVersion("1.0")]
    public class PaymentsController : ApiBaseController
    {
        private readonly IMediator _mediator;
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentsController> _logger;
       
        public PaymentsController(IMediator mediator,IPaymentService paymentService, IMapper mapper, ILogger<PaymentsController> logger)
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
