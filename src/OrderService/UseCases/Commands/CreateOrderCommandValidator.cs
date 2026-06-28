using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;

namespace OrderService.UseCases.Commands
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        private readonly OrderDbContext _context;
        public CreateOrderCommandValidator(OrderDbContext context)
        {
            _context = context;

            // ProductId должен существовать и быть больше нуля, Quantity должен быть больше нуля, а ClientId должен существовать и быть больше нуля
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Количество должно быть больше нуля.");

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Некорректный ID продукта.")
                .MustAsync(ProductExistsAsync).WithMessage("Продукт с таким ID не найден в базе данных!");

            RuleFor(x => x.ClientId)
                .GreaterThan(0).WithMessage("Некорректный ID клиента.")
                .MustAsync(ClientExistsAsync).WithMessage("Клиент с таким ID не найден в базе данных!");
        }

        private async Task<bool> ProductExistsAsync(int productId, CancellationToken cancellationToken)
        {
            return await _context.Products.AnyAsync(p => p.Id == productId, cancellationToken);
        }
        private async Task<bool> ClientExistsAsync(long clientId, CancellationToken cancellationToken)
        {
            return await _context.Clients.AnyAsync(c => c.Id == clientId, cancellationToken);
        }
    }
}