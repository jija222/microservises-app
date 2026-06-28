using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Events;
using OrderService.Models;
namespace OrderService.UseCases.Commands
{
    public class CreateOrderCommand : IRequest<long>
    {
        public int ProductId { get; set; } //Внешний ключ на продукт, который в заказе
        public int Quantity { get; set; }  // Количество продукта в заказе

        public long ClientId { get; set; } // Id клиента, который делает заказ
    }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, long>
    {
        private readonly OrderDbContext _context;
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public CreateOrderCommandHandler(OrderDbContext context,
            ILogger<CreateOrderCommandHandler> logger, IMediator mediator, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }
        public async Task<long> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Начало создания заказа для клиента с Id {request.ClientId}");

            var newOrder = _mapper.Map<Order>(request);
            //newOrder.CreatedDate = DateTime.UtcNow;

            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync(cancellationToken);


            var productPrice = await _context.Products
                .Where(p => p.Id == request.ProductId)
                .Select(p => p.Price) 
                .FirstAsync(cancellationToken); // FirstAsync выбросит ошибку, если не найдет, но Валидатор гарантирует, что найдет

            await _mediator.Publish(new OrderCreatedNotification
            {
                OrderId = newOrder.Id,
                Price = productPrice 
            }, cancellationToken);

            return newOrder.Id;
        }
    }
}
