using MediatR;
using OrderService.Data;

namespace OrderService.UseCases.Queries
{
    public class GetOrderQuery : IRequest<OrderResponse?>
    {
        public long OrderId { get; set; }
    }

    public class OrderResponse
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ClientEmail { get; set; }
        public decimal Price { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class  GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OrderResponse?>
    {
        private readonly OrderDbContext _context;

        public GetOrderQueryHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<OrderResponse?> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.FindAsync(new object[] { request.OrderId}, cancellationToken);
            if (order == null)
            {
                return null;
            }
            return new OrderResponse
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