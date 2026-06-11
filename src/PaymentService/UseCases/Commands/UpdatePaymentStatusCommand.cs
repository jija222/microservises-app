using MediatR;
using PaymentService.Data;
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
        public UpdatePaymentStatusCommandHandler(PaymentDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(UpdatePaymentStatusCommand request, CancellationToken cancellationToken)
        {
            var payment = await _context.Payments.FindAsync(new object[] { request.PaymentId}, cancellationToken);
            if (payment == null) return false;
            payment.Status = request.Status;
            await _context.SaveChangesAsync(cancellationToken);
            // доделать отправку сообщений в кафка
            return true;
        }
    }
}
