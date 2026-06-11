using MediatR;
using PaymentService.Data;
using PaymentService.Models;
namespace PaymentService.UseCases.Commands
{
    public class CreatePaymentCommand : IRequest<long>
    {
        public long OrderId { get; set; }
        public decimal Price { get; set; }
    }

    public class  CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, long>
    {
        private readonly PaymentDbContext _context;
        public CreatePaymentCommandHandler(PaymentDbContext context)
        {
            _context = context;
        }

        public async Task<long> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = new Payment
            {
                OrderId = request.OrderId,
                Price = request.Price
            };
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync(cancellationToken);
            return payment.Id;
        }
    }
}
