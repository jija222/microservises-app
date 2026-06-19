using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;
using System.Runtime.CompilerServices;
using static Confluent.Kafka.ConfigPropertyNames;

namespace NotificationService.Services
{
    public class KafkaConsumerService : BackgroundService 
    {
        private readonly string _topic = "payment_events";
        private IConsumer<Ignore, string> _consumer;
        private readonly ILogger<KafkaConsumerService> _logger;
        private readonly IConfiguration _config;
        private readonly INotificationDispatcher _dispatcher;

        public KafkaConsumerService(
            IKafkaConsumerFactory consumerFactory, 
            INotificationDispatcher dispatcher,
            ILogger<KafkaConsumerService> logger)
        {
            _dispatcher = dispatcher;
            _logger = logger;
            _consumer = consumerFactory.CreateConsumer();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _consumer = null;
                try
                {
                    var config = new ConsumerConfig
                    {
                        BootstrapServers = _config["Kafka:BootstrapServers"],
                        GroupId = _config["Kafka:GroupId"],
                        AutoOffsetReset = AutoOffsetReset.Earliest,
                    };

                    _consumer = new ConsumerBuilder<Ignore, string>(config).Build();

                    _logger.LogInformation("Подключение к Kafka...");

                    _consumer.Subscribe(_topic);

                    _logger.LogInformation($"Начато прослушивание топика {_topic}");

                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var consumeResult = _consumer.Consume(stoppingToken);

                        if (consumeResult?.Message?.Value == null)
                            continue;

                        var message = consumeResult.Message.Value;

                        _logger.LogInformation($"Получено сообщение из Kafka: {message}");

                        await _dispatcher.DispatchAsync(message, stoppingToken);
                    }
                }
                catch (KafkaException ex)
                {
                    _logger.LogWarning($"Kafka недоступна: {ex.Message}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Неожиданная ошибка Kafka consumer");
                }
                finally
                {
                    try
                    {
                        _consumer.Unsubscribe();
                        _consumer.Close();
                    }
                    catch { }
                }

                _logger.LogInformation("Переподключение к Kafka через 5 секунд...");
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
