using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;

        public GetPaymentQueryHandler(PaymentDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaymentResponse?> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
        {
            var payment = await _context.Payments
                .FirstOrDefaultAsync(p => p.Id == request.PaymentId, cancellationToken);

            if (payment == null) return null;

            return _mapper.Map<PaymentResponse>(payment);
        }
    }
}
