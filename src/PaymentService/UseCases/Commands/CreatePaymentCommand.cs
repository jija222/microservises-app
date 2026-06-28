using MediatR;
using PaymentService.Data;
using PaymentService.Models;
using AutoMapper;
namespace PaymentService.UseCases.Commands
{
    /// <summary>
    /// Команда для создания нового платежа со статусом "Не оплачено" (Status = false).
    /// Вызывается синхронно из OrderService через Refit.
    /// </summary>
    public class CreatePaymentCommand : IRequest<long>
    {
        // Идентификатор заказа, к которому привязан платеж
        public long OrderId { get; set; }
        // Сумма к оплате
        public decimal Price { get; set; }
    }

    public class  CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, long>
    {
        private readonly PaymentDbContext _context;
        private readonly IMapper _mapper;
        public CreatePaymentCommandHandler(PaymentDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<long> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = _mapper.Map<Payment>(request);

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync(cancellationToken);
            return payment.Id;
        }
    }
}
