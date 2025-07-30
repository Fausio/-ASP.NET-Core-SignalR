using Microsoft.AspNetCore.SignalR;
using sr.infra;

namespace sr
{
    public interface INotificationService
    {
        Task SendNotificationAsync(string userId, string message);
    }

    public class NotificationService : INotificationService
    {
        private readonly IHubContext<SignalHub> _hubContext;

        public NotificationService(IHubContext<SignalHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendNotificationAsync(string userId, string message)
        {
            await _hubContext.Clients.Group($"user-{userId}").SendAsync("ReceiveNotification", message);
        }
    }
}
