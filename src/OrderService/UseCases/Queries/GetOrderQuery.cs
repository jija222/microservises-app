using MediatR;
using OrderService.Data;

namespace OrderService.UseCases.Queries
{
    public class GetOrderQuery : IRequest<OrderResponсe?>
    {
        public long OrderId { get; set; }
    }

    public class OrderResponсe
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ClientEmail { get; set; }
        public decimal Price { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class  GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OrderResponсe?>
    {
        private readonly OrderDbContext _context;

        public GetOrderQueryHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<OrderResponсe?> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.FindAsync(new object[] { request.OrderId, cancellationToken });
            if (order == null)
            {
                return null;
            }
            return new OrderResponсe
            {
                ProductId = order.ProductId,
                Quantity = order.Quantity,
                ClientEmail = order.ClientEmail,
                Price = order.Price,
                PhoneNumber = order.PhoneNumber
            };
        }
    }
}