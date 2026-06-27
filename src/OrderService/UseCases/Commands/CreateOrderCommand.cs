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
        public int Quantity { get; set; } // Количество продукта в заказе
        public string ClientEmail { get; set; } // Email клиента, который сделал заказ
        public decimal Price { get; set; } // Цена продукта в заказе
        public string PhoneNumber { get; set; } // Номер телефона клиента, который сделал заказ
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
            _logger.LogInformation($"Начало создания заказа для email {request.ClientEmail}");
            var newOrder = _mapper.Map<Order>(request); // Добавляем маппинг с помощью AutoMapper

            _context.Orders.Add(newOrder);

            await _context.SaveChangesAsync(cancellationToken);
            var product = await _context.Products
              .FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);
            _logger.LogInformation($"Заказ с id {newOrder.Id} успешно создан для email {request.ClientEmail}");

            await _mediator.Publish(new OrderCreatedNotification
            {
                OrderId = newOrder.Id,

               Price = product?.Price ?? 0m
            }, cancellationToken);

            return newOrder.Id;
        }
    }
}
