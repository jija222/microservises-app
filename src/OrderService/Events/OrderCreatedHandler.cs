using MediatR;
using OrderService.Clients;

namespace OrderService.Events
{
    // Этот класс автоматически сработает, когда кто-то опубликует OrderCreatedNotification
    public class OrderCreatedHandler : INotificationHandler<OrderCreatedNotification>
    {
        private readonly IPaymentClient _paymentClient;

        public OrderCreatedHandler(IPaymentClient paymentClient)
        {
            _paymentClient = paymentClient;
        }

        public async Task Handle(OrderCreatedNotification notification, CancellationToken cancellationToken)
        {
            var request = new PaymentCreateRequest { OrderId = notification.OrderId, Price = notification.Price };
            await _paymentClient.CreatePaymentAsync(request);
        }
    }
}