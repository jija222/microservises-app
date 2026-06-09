namespace OrderService.DTOs
{
    public class CreateOrderRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string ClientEmail { get; set; }
        public decimal Price { get; set; }
        public string PhoneNumber { get; set; }
    }
}
