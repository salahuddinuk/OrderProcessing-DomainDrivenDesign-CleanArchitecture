using Microsoft.AspNetCore.Mvc;
using OrderProcessing.Application.DTOs;
using OrderProcessing.Application.Services;

namespace OrderProcessing.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            if (request == null)
            {
                _logger.LogError("CreateOrder request is null.");
                return BadRequest("Invalid order data.");
            }
            _logger.LogInformation("Received CreateOrder request with {ItemCount} items.", request?.Items?.Count ?? 0);

            var orderId = await _orderService.CreateOrderAsync(request);
            return CreatedAtAction(nameof(GetOrderById), new { id = orderId }, new { orderId });
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            _logger.LogInformation("Fetching order with ID: {OrderId}", id);
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                _logger.LogWarning("Order not found: {OrderId}", id);
                return NotFound();
            }
            return Ok(order);
        }
    }
}
