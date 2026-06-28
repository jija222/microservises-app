namespace PaymentService.Models
{
    /// <summary>
    /// Сущность для базы данных. Отвечает исключительно за хранение платежа в PostgreSQL.
    /// Не объединяется с Event, чтобы не "светить" структуру БД наружу и иметь возможность добавлять служебные поля EF Core без поломки контрактов.
    /// </summary>
    public class Payment
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public decimal Price { get; set; }
        public bool Status { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}