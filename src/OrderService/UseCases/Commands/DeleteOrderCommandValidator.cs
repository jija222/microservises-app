using FluentValidation;

namespace OrderService.UseCases.Commands
{
    public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderCommandValidator()
        {
            // ID не может быть меньше или равен нулю
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("ID заказа должен быть больше нуля!");
        }
    }
}