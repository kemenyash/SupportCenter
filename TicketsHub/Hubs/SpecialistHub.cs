using Microsoft.AspNetCore.SignalR;
using Schemas.Entities;
using System.Net.Sockets;
using TicketsHub.Services;

namespace TicketsHub.Hubs
{
    public class SpecialistHub : Hub
    {

        public async Task ReceiveSpecialistAssigned(string userId, Ticket ticket)
        {
            await Clients.Group(userId).SendAsync("ReceiveSpecialistAssigned", ticket);
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

            if (TicketsProcesser.Specialists is null) TicketsProcesser.Specialists = new System.Collections.Concurrent.ConcurrentDictionary<string, Specialist>();
            if (!TicketsProcesser.Specialists.ContainsKey(userId))
            {
                var specialist = new Specialist
                {
                    AssignedTickets = new List<Ticket>(),
                    Email = $"specialist-{TicketsProcesser.Specialists.Count + 1}@site.com",
                    UserId = userId,
                    Name = $"Specialist #{TicketsProcesser.Specialists.Count + 1}",
                    Workload = 0
                };
                TicketsProcesser.Specialists.TryAdd(userId, specialist);

                await Clients.Group(userId).SendAsync("ReceiveSpecialistProfile", specialist);
            }
        }
    }
}
