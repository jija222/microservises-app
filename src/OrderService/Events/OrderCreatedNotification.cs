using MediatR;

namespace OrderService.Events
{
    public class OrderCreatedNotification : INotification
    {
        public long OrderId { get; set; }
        public decimal Price { get; set; }
    }
}