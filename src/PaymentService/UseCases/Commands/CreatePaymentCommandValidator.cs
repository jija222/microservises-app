using FluentValidation;

namespace PaymentService.UseCases.Commands
{
    public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
    {
        public CreatePaymentCommandValidator()
        {
            RuleFor(x => x.OrderId).GreaterThan(0).WithMessage("ID заказа должен быть больше нуля.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Сумма платежа должна быть больше нуля.");
        }
    }
}