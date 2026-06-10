using Microsoft.AspNetCore.Mvc;
using OrderService.Data;
using OrderService.Models;
using OrderService.DTOs;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderDbContext _context;

        public OrdersController(OrderDbContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
        public IActionResult CreateOrder([FromBody] CreateOrderRequest request)
        {

            var newOrder = new Order
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                ClientEmail = request.ClientEmail,
                PhoneNumber = request.PhoneNumber,
                Price = request.Price
            };

            _context.Orders.Add(newOrder);
            _context.SaveChanges();
            return Ok(new { OrderId = newOrder.Id });
        }

        [HttpGet("{order_id}")]
        public IActionResult GetOrder(long order_id)
        {
            var order = _context.Orders.Find(order_id);
            if(order == null)
            {
                return NotFound(new { Message = $"Заказ с ID {order_id} не найден" });
            }

            return Ok(new
            {
                productId = order.ProductId,
                quantity = order.Quantity,
                ClientEmail = order.ClientEmail,
                price = order.Price,
                phoneNumber = order.PhoneNumber
            });
        }

        [HttpDelete("{order_id}")]
        public IActionResult DeleteOrder(long order_id)
        {
            var order = _context.Orders.Find(order_id);

            if(order == null)
            {
                return NotFound(new { Message = $"Заказ с номером {order_id} не найден" });
            }
            _context.Orders.Remove(order);
            _context.SaveChanges();
            return Ok(new { Message = $"Заказ с номером {order_id} успешно удален" });
        }
    }
}