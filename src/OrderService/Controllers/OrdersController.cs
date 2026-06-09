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
    }
}