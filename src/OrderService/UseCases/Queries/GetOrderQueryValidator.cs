using FluentValidation;
using OrderService.UseCases.Queries;

namespace OrderService.UseCases.Queries
{
    public class GetOrderQueryValidator : AbstractValidator<GetOrderQuery>
    {
        public GetOrderQueryValidator()
        {
            // ID не может быть меньше или равен нулю
            RuleFor(x => x.OrderId).GreaterThan(0).WithMessage("ID заказа должен быть больше нуля!");
        }
    }
}