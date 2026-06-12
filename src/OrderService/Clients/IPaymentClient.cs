using Refit;

namespace OrderService.Clients
{
    public class PaymentCreateRequest
    {
        public long OrderId { get; set; }
        public decimal Price { get; set; }
    }

    public interface IPaymentClient
    {
        [Post("/api/payments/create")]
        Task<object> CreatePaymentAsync([Body] PaymentCreateRequest request);
    }
}
