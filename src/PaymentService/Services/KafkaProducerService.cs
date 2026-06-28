using Confluent.Kafka;
using System.Text.Json;

namespace PaymentService.Services
{
    public interface IKafkaProducerService
    {
        Task SendPaymentEventAsync(string topic, object message);
    }

    public class KafkaProducerService : IKafkaProducerService, IDisposable
    {
        private readonly IProducer<Null, string> _producer;
        private readonly ProducerConfig _config;
        private readonly ILogger<KafkaProducerService> _logger;

        public KafkaProducerService(IConfiguration config, ILogger<KafkaProducerService> logger)
        {
            _config = new ProducerConfig
            {
                BootstrapServers = config.GetValue<string>("Kafka:BootstrapServers")
            };
            _logger = logger;
            _producer = new ProducerBuilder<Null, string>(_config).Build();
        }

        public async Task SendPaymentEventAsync(string topic, object message)
        {
            var jsonMessage = JsonSerializer.Serialize(message);

            var kafkaMessage = new Message<Null, string> { Value = jsonMessage };

            var deliveryResult = await _producer.ProduceAsync(topic, kafkaMessage);

            _logger.LogInformation($"Событие отправлено в Kafka. Topic: {topic}, Offset: {deliveryResult.Offset}");
        }
        public void Dispose()
        {
            // Flush гарантирует, что все сообщения, которые остались в памяти, будут отправлены перед выключением
            _producer.Flush(TimeSpan.FromSeconds(10));
            _producer.Dispose();
        }
    }
}