using Newtonsoft.Json;
using Schemas.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Schemas.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public PriorityLevel Priority { get; set; }
        public TicketStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public User CreatedBy { get; set; }
        public Specialist? AssignedSpecialist { get; set; }
        public List<Comment>? Comments { get; set; }

        public async Task AddComment(Comment comment, string url)
        {
            Comments.Add(comment);

            using (var httpClient = new HttpClient())
            {
                var jsonData = JsonConvert.SerializeObject(comment);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, content);
            }
        }

    }
}
