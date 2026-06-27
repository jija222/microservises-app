using MediatR;
using Microsoft.EntityFrameworkCore;
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
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == request.OrderId, cancellationToken);
            if (order == null) {
                return false; // Если вернем false, то это будет означать, что заказ не найден и удаление не произошло
                //Будет выведено "Заказ с Id {order_id} не удален"
            }
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync(cancellationToken);
            return true; // Если вернем true, то это будет означать, что заказ успешно удален
        }
    }
}
