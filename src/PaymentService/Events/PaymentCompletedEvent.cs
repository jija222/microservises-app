namespace PaymentService.Events
{
    public class PaymentCompletedEvent
    {
        public long PaymentId { get; set; }
        public long OrderId { get; set; }
        public decimal Price { get; set; }
        public bool Status { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
