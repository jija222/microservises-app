using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data;

namespace PaymentService.UseCases.Queries
{
    public class GetPaymentQueryValidator : AbstractValidator<GetPaymentQuery>
    {
        private readonly PaymentDbContext _context;

        public GetPaymentQueryValidator(PaymentDbContext context)
        {
            _context = context;

            RuleFor(x => x.PaymentId)
                .GreaterThan(0).WithMessage("ID платежа должен быть больше нуля.")
                .MustAsync(PaymentExistsAsync).WithMessage("Платеж с таким ID не найден в базе данных!");
        }

        private async Task<bool> PaymentExistsAsync(long paymentId, CancellationToken cancellationToken)
        {
            return await _context.Payments.AnyAsync(p => p.Id == paymentId, cancellationToken);
        }
    }
}