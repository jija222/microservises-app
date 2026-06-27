namespace OrderService.Models
{
    public class Product
    {
        public long Id { get; set; }
        public decimal Price { get; set; }

        // Связь один-ко-многим (Один продукт может быть в нескольких заказах)
        public ICollection<Order> Orders { get; set; }
    }
}