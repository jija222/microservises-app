using MediatR;
using PaymentService.Data;
using PaymentService.Events;
using PaymentService.Services;
namespace PaymentService.UseCases.Commands
{
    public class UpdatePaymentStatusCommand : IRequest<bool>
    {
        public long PaymentId { get; set; }
        public bool Status { get; set; }
    }

    public class UpdatePaymentStatusCommandHandler : IRequestHandler<UpdatePaymentStatusCommand, bool>
    {
        private readonly PaymentDbContext _context;
        private readonly IKafkaProducerService _kafkaProducer;
        public UpdatePaymentStatusCommandHandler(PaymentDbContext context, IKafkaProducerService kafkaProducer)
        {
            _context = context;
            _kafkaProducer = kafkaProducer;
        }
        public async Task<bool> Handle(UpdatePaymentStatusCommand request, CancellationToken cancellationToken)
        {
            var payment = await _context.Payments.FindAsync(new object[] { request.PaymentId }, cancellationToken);
            if (payment == null) return false;
            payment.Status = request.Status;
            await _context.SaveChangesAsync(cancellationToken);

            if (payment.Status)
            {
                var paymentEvent = new PaymentCompletedEvent
                {
                    PaymentId = payment.Id,
                    OrderId = payment.OrderId,
                    Price = payment.Price,
                    Status = payment.Status,
                    CreatedAt = DateTime.UtcNow
                };
                await _kafkaProducer.SendPaymentEventAsync("payment_events",paymentEvent);
            }

            return true;
        }
    }
}


