using Microsoft.AspNetCore.SignalR;
using Schemas.Entities;
using TicketsHub.Services;

namespace TicketsHub.Hubs
{
    public class AdminHub : Hub
    {

        public async Task NotifyAdmin(string userId, Report report)
        {
            await Clients.Group(userId).SendAsync("ReceiveReport", report);
        }

        public override async Task OnConnectedAsync()
        {

            var httpContext = Context.GetHttpContext();
            var userId = httpContext?.Request.Query["userId"].ToString();

            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }
            await base.OnConnectedAsync();

            if (TicketsProcesser.Admins is null) TicketsProcesser.Admins = new System.Collections.Concurrent.ConcurrentDictionary<string, Admin>();
            if (!TicketsProcesser.Admins.ContainsKey(userId))
            {
                var admin = new Admin
                {
                    AllDepartmentTickets = new List<Ticket>(),
                    Email = $"admin-{TicketsProcesser.Admins.Count + 1}@email.com",
                    Name = $"admin-{TicketsProcesser.Admins.Count + 1}",
                    UserId = userId
                };
                TicketsProcesser.Admins.TryAdd(userId, admin);
                Clients.Group(userId).SendAsync("ReceiveAdminProfile", admin);
            }
        }
    }
}
