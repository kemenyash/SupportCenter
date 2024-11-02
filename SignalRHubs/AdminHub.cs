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
    public class AdminHub : BaseHub
    {
        public async Task SendReportToSpecificAdmin(string userId, Report report)
        {
            await Clients.Group(userId).SendAsync("ReceiveReport", report);
        }
    }
}
