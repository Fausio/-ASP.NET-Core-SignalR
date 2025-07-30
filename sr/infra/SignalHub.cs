using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace sr.infra
{
    public class SignalHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var email = Context.User.Identity.Name;

            if (!string.IsNullOrEmpty(email))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, email);
            }

            await base.OnConnectedAsync();
        }

        public async Task SendNotification(string userId, string message)
        {
            await Clients.Group(userId).SendAsync("ReceiveNotification", message);
        }
    }
}
