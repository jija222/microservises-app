using Microsoft.AspNetCore.Mvc;
using OrderService.Data;
using OrderService.Models;
using MediatR;
using OrderService.UseCases.Commands;
using OrderService.UseCases.Queries;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrdersController(OrderDbContext context, IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        { 
            var orderId = await _mediator.Send(command);
            return Ok(new { OrderId = orderId });
        }

        [HttpGet("{order_id}")]
        public async Task<IActionResult> GetOrder(long order_id)
        {
            var query = new GetOrderQuery { OrderId = order_id };
            var result = await _mediator.Send(query);
            if(result == null)
            {
                return NotFound(new { Message = $"Заказ с Id {order_id} не найден" });
            }
            return Ok(result);
        }

        [HttpDelete("{order_id}")]
        public async Task<IActionResult> DeleteOrder(long order_id)
        {
            var command = new DeleteOrderCommand { OrderId = order_id };
            var isDeleted = await _mediator.Send(command);
            if(!isDeleted)
            {
                return NotFound(new { Message = $"Заказ с Id {order_id} не удален" });
            }
            return Ok(new { Message = $"Заказ с Id {order_id} успешно удален" });
        }
    }
}