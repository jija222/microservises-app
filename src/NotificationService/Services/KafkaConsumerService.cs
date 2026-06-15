using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;
using System.Runtime.CompilerServices;

namespace NotificationService.Services
{
    public class KafkaConsumerService : BackgroundService 
    {
        private readonly string _topic = "payment_events";
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly ILogger<KafkaConsumerService> _logger;
        private readonly IHubContext<NotificationHub> _hubContext;

        public KafkaConsumerService(ILogger<KafkaConsumerService> logger, IHubContext<NotificationHub> hubContext,
            IConfiguration configuration)
        {
            _hubContext = hubContext;
            _logger = logger;
            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = configuration.GetValue<string>("Kafka:BootstrapServers"),
                GroupId = configuration.GetValue<string>("Kafka:GroupId"),
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer.Subscribe(_topic);
            _logger.LogInformation($"Начато прослушивание топика {_topic}");
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var consumeResult = _consumer.Consume(stoppingToken);
                    var message = consumeResult.Message.Value;
                    _logger.LogInformation($"Получено сообщение из Kafka: {message}");

                    await _hubContext.Clients.All.SendAsync("ReceiveNotification", message, cancellationToken: stoppingToken);
                }
            }
            catch (ConsumeException ex)
            {
                _logger.LogWarning($"Ошибка чтения из Kafka: {ex.Error.Reason}");
                await Task.Delay(2000, stoppingToken);
            }
            catch (OperationCanceledException) { }
            finally
            {
                _consumer.Close();
            }
        }
    }
}
