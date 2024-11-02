using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace TicketsHub.Hubs
{
    public abstract class BaseHub : Hub
    {
        protected string userId;
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            userId = httpContext?.Request.Query["userId"].ToString();

            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }
            await base.OnConnectedAsync();
        }
    }
}
