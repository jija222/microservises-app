namespace OrderService.Models
{
    public class Client
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        // Связь один-ко-многим (У одного клиента много заказов)
        public ICollection<Order> Orders { get; set; }
    }
}