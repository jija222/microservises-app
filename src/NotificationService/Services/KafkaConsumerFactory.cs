using Confluent.Kafka;

namespace NotificationService.Services
{
    public interface IKafkaConsumerFactory
    {
        IConsumer<Ignore, string> CreateConsumer();
    }
    public class KafkaConsumerFactory : IKafkaConsumerFactory
    {
        private readonly ConsumerConfig _config;

        public KafkaConsumerFactory(IConfiguration configuration)
        {
            _config = new ConsumerConfig
            {
                BootstrapServers = configuration.GetValue<string>("Kafka:BootstrapServers"),
                GroupId = configuration.GetValue<string>("Kafka:GroupId"),
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
        }

        public IConsumer<Ignore, string> CreateConsumer()
        {
            return new ConsumerBuilder<Ignore, string>(_config).Build();
        }
    }
}
