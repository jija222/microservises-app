using NotificationService.Hubs;
using NotificationService.Services;
using static NotificationService.Services.INotificationDispatcher;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddSingleton<IKafkaConsumerFactory, KafkaConsumerFactory>();
builder.Services.AddSingleton<INotificationDispatcher, SignalRNotificationDispatcher>();
builder.Services.AddHostedService<KafkaConsumerService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.MapHub<NotificationHub>("/notifications");

app.MapControllers();

app.Run();