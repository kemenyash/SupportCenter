using Microsoft.AspNetCore.SignalR;
using Schemas.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SignalRHubs
{
    public class CustomerHub : Hub
    {

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.User.Identities.First();
        }
        public async Task RegisterCustomer(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }


        public async Task TicketChangedForCustomer(string userId, Ticket ticket)
        {
            await Clients.Group(userId).SendAsync("ReceiveTicketChanged", ticket);
        }
    }
}
