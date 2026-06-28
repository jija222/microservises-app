using MediatR;

namespace PaymentService.Events
{
    /// <summary>
    /// Событие для Kafka. Отвечает за передачу данных в другие микросервисы (например, NotificationService).
    /// Не объединяется с моделью БД для независимого развития: формат сообщений (контракт) может меняться отдельно от структуры таблиц.
    /// </summary>
    public class PaymentCompletedEvent : INotification
    {
        public long PaymentId { get; set; }
        public long OrderId { get; set; }
        public decimal Price { get; set; }
        public bool Status { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
