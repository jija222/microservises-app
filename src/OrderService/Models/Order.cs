namespace OrderService.Models
{
    public class Order
    {
        public long Id { get; set; } // Первичный ключ
        public int Quantity { get; set; } // Количество товара в заказе
        public DateTime CreatedDate { get; set; }  = DateTime.UtcNow; // Дата создания заказа

        // Внешние ключи (Связи с другими таблицами)
        public long ClientId { get; set; }
        public Client Client { get; set; }

        public long ProductId { get; set; }
        public Product Product { get; set; }
    }
}