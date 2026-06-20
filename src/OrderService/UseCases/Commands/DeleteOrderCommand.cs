using System;
using MediatR;
using OrderService.Data;
namespace OrderService.UseCases.Commands
{
    public class DeleteOrderCommand : IRequest<bool>
    {
        public long OrderId { get; set; }
    }

    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, bool>
    {
        private readonly OrderDbContext _context;

        public DeleteOrderCommandHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.FindAsync(new object[] { request.OrderId }, cancellationToken);
            if (order == null) {
                return false;
            }
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
