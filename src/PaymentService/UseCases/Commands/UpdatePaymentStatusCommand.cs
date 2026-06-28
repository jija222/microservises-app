using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Events;

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
        private readonly IMediator _mediator; // Внедряем Медиатор
        private readonly IMapper _mapper;

        public UpdatePaymentStatusCommandHandler(PaymentDbContext context, IMediator mediator, IMapper mapper)
        {
            _context = context;
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdatePaymentStatusCommand request, CancellationToken cancellationToken)
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.Id == request.PaymentId, cancellationToken);

            if (payment == null) return false; // Статус оплаты у заказа не поменяется

            payment.Status = request.Status;
            await _context.SaveChangesAsync(cancellationToken);

            
            if (payment.Status)
            {
                var paymentEvent = _mapper.Map<PaymentCompletedEvent>(payment);

                
                await _mediator.Publish(paymentEvent, cancellationToken);
            }

            return true; // Статус оплаты поменяется
        }
    }
}