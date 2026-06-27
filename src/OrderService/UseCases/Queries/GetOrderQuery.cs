using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;

//Рефакторинг сделан с использованием AutoMapper для маппинга сущности Order в OrderResponse
namespace OrderService.UseCases.Queries
{
    public class GetOrderQuery : IRequest<OrderResponse?>
    {
        public long OrderId { get; set; }
    }

    public class OrderResponse
    {
        public long OrderId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }
        public long ClientId { get; set; }
    }

    public class  GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OrderResponse?>
    {
        private readonly OrderDbContext _context;
        private readonly IMapper _mapper;

        public GetOrderQueryHandler(OrderDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OrderResponse?> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.Id == request.OrderId, cancellationToken);
            if (order == null)
            {
                return null;
            }
            return _mapper.Map<OrderResponse>(order);
        }
    }
}