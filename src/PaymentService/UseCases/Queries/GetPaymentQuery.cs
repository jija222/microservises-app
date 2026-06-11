using MediatR;
using PaymentService.Data;

namespace PaymentService.UseCases.Queries
{
    public class GetPaymentQuery : IRequest<PaymentResponse?>
    {
        public long PaymentId { get; set; }
    }

    public class PaymentResponse
    {
        public decimal Price { get; set; }
        public bool Status { get; set; }
        public DateTime DateCreate { get; set; }
    }

    public class GetPaymentQueryHandler : IRequestHandler<GetPaymentQuery, PaymentResponse?>
    {
        private readonly PaymentDbContext _context;

        public GetPaymentQueryHandler(PaymentDbContext context)
        {
            _context = context;
        }

        public async Task<PaymentResponse?> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
        {
            var payment = await _context.Payments.FindAsync(new object[] { request.PaymentId }, cancellationToken);
            if (payment == null) return null;

            return new PaymentResponse
            {
                Price = payment.Price,
                Status = payment.Status,
                DateCreate = payment.CreatedAt
            };
        }
    }
}
