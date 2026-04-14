using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DigitalBankingBacknend.Hubs
{
    //[Authorize]
    public class NotificationHub : Hub
    {
        // Called automatically when Angular connects
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        // Called automatically when Angular disconnects
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
