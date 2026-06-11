using MediatR;
using OrderService.Data;
using OrderService.Models;
namespace OrderService.UseCases.Commands
{
    public class CreateOrderCommand : IRequest<long>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ClientEmail { get; set; }
        public decimal Price { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, long>
    {
        private readonly OrderDbContext _context;
        public CreateOrderCommandHandler(OrderDbContext context)
        {
            _context = context;
        }
        public async Task<long> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var newOrder = new Order
            {
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                ClientEmail = request.ClientEmail,
                Price = request.Price,
                PhoneNumber = request.PhoneNumber
            };
            _context.Orders.Add(newOrder);

            await _context.SaveChangesAsync(cancellationToken);

            return newOrder.Id;
        }
    }
}
