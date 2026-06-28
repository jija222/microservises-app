using MediatR;
using PaymentService.Events;
using PaymentService.Services;

// Этот класс автоматически вызывается MediatR, когда в UpdatePaymentStatusCommandHandler срабатывает _mediator.Publish

namespace PaymentService.UseCases.EventHandlers
{
    public class PaymentCompletedKafkaHandler : INotificationHandler<PaymentCompletedEvent>
    {
        private readonly IKafkaProducerService _kafkaProducer;

        public PaymentCompletedKafkaHandler(IKafkaProducerService kafkaProducer)
        {
            _kafkaProducer = kafkaProducer;
        }

        public async Task Handle(PaymentCompletedEvent notification, CancellationToken cancellationToken)
        {
            await _kafkaProducer.SendPaymentEventAsync("payment_events", notification);
        }
    }
}