using FluentValidation;
using OrderService.UseCases.Commands;

namespace OrderService.UseCases.Commands
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            // ProductId должен быть больше нуля, Quantity должен быть больше нуля, а ClientEmail должен быть валидным email адресом
            RuleFor(x => x.ProductId).GreaterThan(0).WithMessage("Некорректный ID продукта.");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Количество должно быть больше нуля.");
            RuleFor(x => x.ClientEmail).NotEmpty().EmailAddress().WithMessage("Укажите правильный Email.");
        }
    }
}