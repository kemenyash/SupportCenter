using Microsoft.AspNetCore.SignalR;
using Schemas.Entities;
using TicketsHub.Services;

namespace TicketsHub.Hubs
{
    public class CustomerHub : BaseHub
    {
        public async Task NotifyCustomer(string userId, Comment comment)
        {
            await Clients.Group(userId).SendAsync("AddedNewComment", comment);
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

            if (TicketsProcesser.Customers is null) TicketsProcesser.Customers = new System.Collections.Concurrent.ConcurrentDictionary<string, Customer>();
            if (!TicketsProcesser.Customers.ContainsKey(userId))
            {
                var customer = new Customer
                {
                    Email = $"customer-{TicketsProcesser.Customers.Count + 1}@email.com",
                    Name = $"customer-{TicketsProcesser.Customers.Count + 1}",
                    Tickets = new System.Collections.ObjectModel.ObservableCollection<Ticket>(),
                    UserId = userId
                };
                TicketsProcesser.Customers.TryAdd(userId, customer);

                await Clients.Group(userId).SendAsync("ReceiveCustomerProfile", customer);
            }
        }
    }
}
