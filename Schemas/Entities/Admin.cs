using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Schemas.Entities
{
    public class Admin : User
    {
        private static int reportCounter;
        public Admin() 
        {
            AllDepartmentTickets = new List<Ticket>();
        }
        public List<Ticket> AllDepartmentTickets { get; set; }

        public async Task<Report> GenerateReport(string url)
        {
            reportCounter++;

            var data = await new WebClient().DownloadStringTaskAsync(url);
            AllDepartmentTickets = JsonConvert.DeserializeObject<List<Ticket>>(data);

            var report = new Report
            {
                CreatedAt = DateTime.UtcNow,
                Content = $"Created {AllDepartmentTickets.Count} tickets",
                Title = $"Tickets report #{reportCounter}"
            };

            if (AllDepartmentTickets != null && AllDepartmentTickets.Count > 0)
            {
                report.Content += $"\r\nTickets list:";
                int count = 0;
                foreach (var ticket in AllDepartmentTickets)
                {
                    count++;
                    report.Content += $"\r\n#{count} {ticket.Title} created at {ticket.CreatedAt}";
                }
            }

            return report;
        }
    }
}
