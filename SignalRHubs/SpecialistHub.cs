using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Schemas.Entities;

namespace SignalRHubs
{
    public class SpecialistHub : BaseHub
    {
        public async Task TicketAssignedToSpecialist(string userId, Ticket ticket)
        {
            await Clients.Group(userId).SendAsync("ReceiveTicketAssigned", ticket);
        }
    }
}
