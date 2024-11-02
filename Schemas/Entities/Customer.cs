using Newtonsoft.Json;
using Schemas.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Schemas.Entities
{
    public class Customer : User
    {
        public delegate void TicketAction(Ticket ticket);
        public TicketAction AddNewTicketToScope;
        public ObservableCollection<Ticket> Tickets { get; set; }

        public async Task CreateTicket(string url, Ticket ticket)
        {
            Tickets.Add(ticket);
            using (var httpClient = new HttpClient()) 
            {
                var jsonData = JsonConvert.SerializeObject(ticket, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, content);
            }
        }
    }
}
