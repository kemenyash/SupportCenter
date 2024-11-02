using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalRHubs
{
    public class BaseHub<T> : Hub
    {
        protected readonly Dictionary<int, T> registeredUsers;

        public BaseHub() => registeredUsers = new Dictionary<int, T>();

        public override async Task OnConnectedAsync()
        {

            var httpContext = Context.GetHttpContext();
            var userId = httpContext.Request.Query["userId"].ToString();

            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }

            await base.OnConnectedAsync();
        }
    }
}
