namespace Skinet.API.Controllers
{

    public class OrdersController : ApiBaseController
    {
        /*
        private readonly IOrderService _orderService;
        private readonly IMediator _mediator;

        public OrdersController(IOrderService orderService, IMediator mediator)
        {
            _orderService = orderService;
            _mediator = mediator;
        }

       
        [HttpPost]
        public async Task<ActionResult<BaseResponse<OrderModel>>> CreateOrder(CreateOrderCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }


        
        [HttpGet]
        public async Task<ActionResult<BaseResponse<List<OrderModel>>>> GetOrdersForUser()
        {
            var response = await _mediator.Send(new GetOrdersForUserQuery());
            return Ok(response);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<OrderModel>> GetSpecificOrderForUser(int id)
        {
            var response = await _mediator.Send(new GetByIdSpecificOrderForUserQuery(id));
            return Ok(response);
        }

        [HttpGet("delivery-methods")]
        public async Task<ActionResult<DeliveryMethod>> GetDeliveryMethods()
        {
            var deliveries = await _orderService.GetDeliveryMethodsAsync();
            if (deliveries is null)
            {
                return BadRequest(new ApiResponse(404, $"There is no Delivery Methods"));
            }
            return Ok(deliveries);
        }
        */
    }
}

