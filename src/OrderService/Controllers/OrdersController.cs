using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.UseCases.Commands;
using OrderService.UseCases.Queries;
//Рефкторинг сделан, добавлена валидация для команд и запросов
namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly OrderDbContext _context;

        public OrdersController(IMediator mediator, OrderDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {
            var validator = new CreateOrderCommandValidator(_context);
            var validationResult = await validator.ValidateAsync(command);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            var orderId = await _mediator.Send(command);

            return Ok(orderId);
        }

        [HttpGet("{order_id}")]
        public async Task<IActionResult> GetOrder(long order_id)
        {
            var query = new GetOrderQuery { OrderId = order_id };

            var validator = new GetOrderQueryValidator();
            var validationResult = validator.Validate(query);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            var result = await _mediator.Send(query);
            if(result == null)
            {
                return NotFound(new { Message = $"Заказ с Id {order_id} не найден" }); // глянуть надо ли возвращать сообщение об ошибке
            }
            return Ok(result);
        }

        [HttpDelete("{order_id}")]
        public async Task<IActionResult> DeleteOrder(long order_id)
        {
            var command = new DeleteOrderCommand { OrderId = order_id };

            var validator = new DeleteOrderCommandValidator();
            var validationResult = validator.Validate(command);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors); 
            }

            var isDeleted = await _mediator.Send(command);
            if(!isDeleted)
            {
                return NotFound(new { Message = $"Заказ с Id {order_id} не удален" }); // это тоже глянуть надо

            }
            return Ok(new { Message = $"Заказ с Id {order_id} успешно удален" }); // посмотри надо ли возвращать сообщение об успешном удалении
        }
    }
}