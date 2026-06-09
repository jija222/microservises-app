namespace OrderService.Models
{
    public class  Order 
    {
        public long Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ClientEmail { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}