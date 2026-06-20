using MediatR;
using OrderService.Data;
using OrderService.Models;
using OrderService.Clients;
using Microsoft.Extensions.Logging;
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
        private readonly IPaymentClient _paymentClient;
        private readonly ILogger _logger;
        public CreateOrderCommandHandler(OrderDbContext context, IPaymentClient paymentClient, 
            ILogger<CreateOrderCommandHandler> logger)
        {
            _context = context;
            _paymentClient = paymentClient;
            _logger = logger;
        }
        public async Task<long> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Начало создания заказа для email {request.ClientEmail}");
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
            _logger.LogInformation($"Заказ с id {newOrder.Id} успешно создан для email {request.ClientEmail}");
            try
            {
                var paymentRequest = new PaymentCreateRequest
                {
                    OrderId = newOrder.Id,
                    Price = newOrder.Price,
                };
                await _paymentClient.CreatePaymentAsync(paymentRequest);
                _logger.LogInformation($"Оплата для заказа {newOrder.Id} успешно создана");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при создании оплаты для заказа {newOrder.Id}");
            }


            return newOrder.Id;
        }
    }
}
