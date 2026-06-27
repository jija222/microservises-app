using Confluent.Kafka;
using System.Text.Json;


namespace PaymentService.Services
{
    public interface IKafkaProducerService
    {
        Task SendPaymentEventAsync(string topic, object message);
    }
    public class KafkaProducerService : IKafkaProducerService
    {
        private readonly ProducerConfig _config;
        private readonly ILogger<KafkaProducerService> _logger;
        public KafkaProducerService(IConfiguration config, ILogger<KafkaProducerService> logger )
        {
            _config = new ProducerConfig
            {
                BootstrapServers = config.GetValue<string>("Kafka:BootstrapServers")
            };
            _logger = logger;
        }
        public async Task SendPaymentEventAsync(string topic, object message)
        {
            var delivery = $"topic: {topic}, message: {JsonSerializer.Serialize(message)}";
            var jsonMessage = JsonSerializer.Serialize(message);
            try
            {
                using var producer = new ProducerBuilder<Null, string>(_config).Build();

                var kafkaMessage = new Message<Null, string> { Value = jsonMessage };

                var deliveryResult = await producer.ProduceAsync(topic, kafkaMessage);
                _logger.LogInformation($"Событие оплаты отправлено в Kafka, {delivery}");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Возникла ошибка при отправке события оплаты в Kafka, {delivery}");
            }          
        }
    }
}
