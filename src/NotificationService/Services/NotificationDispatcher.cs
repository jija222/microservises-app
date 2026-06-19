using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Services
{
    public interface INotificationDispatcher
    {
        Task DispatchAsync(string message, CancellationToken cancellationToken);
        public class SignalRNotificationDispatcher : INotificationDispatcher
        {
            private readonly IHubContext<NotificationHub> _hubContext;

            public SignalRNotificationDispatcher(IHubContext<NotificationHub> hubContext)
            {
                _hubContext = hubContext;
            }
            public async Task DispatchAsync(string message, CancellationToken cancellationToken)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveNotification", message, cancellationToken);
            }
        }
    }
}
