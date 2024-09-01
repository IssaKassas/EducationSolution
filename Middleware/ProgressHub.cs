using Microsoft.AspNetCore.SignalR;

namespace WebApp.Middleware
{
    public class ProgressHub: Hub
    {
        public async Task SendProgressUpdate(string userId, int progress)
        {
            await Clients.All.SendAsync("SendProgressUpdate", progress);
        }
    }
}
