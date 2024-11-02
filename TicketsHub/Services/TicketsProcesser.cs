using Microsoft.AspNetCore.SignalR;
using Schemas.Entities;
using System.Collections.Concurrent;
using TicketsHub.Hubs;

namespace TicketsHub.Services
{
    public class TicketsProcesser
    {
        private IHubContext<SpecialistHub> specialistHub;
        private IHubContext<CustomerHub> customerHub;

        public static List<Ticket> Tickets { get; set; }
        public static ConcurrentDictionary<string, Specialist> Specialists;
        public static ConcurrentDictionary<string, Customer> Customers;
        public static ConcurrentDictionary<string, Admin> Admins;

        public TicketsProcesser(IHubContext<SpecialistHub> specialistHub, 
                                IHubContext<CustomerHub> customerHub) 
        {
            this.specialistHub = specialistHub;
            this.customerHub = customerHub;
            if(Specialists is null) 
                Specialists = new ConcurrentDictionary<string, Specialist>();

            if(Customers is null)
                Customers = new ConcurrentDictionary<string, Customer>();

            if(Admins is null)
                Admins = new ConcurrentDictionary<string, Admin>();
        }

        public async Task AddNewComment(Comment comment)
        {
            await customerHub.Clients.Group(comment.CreatedBy.UserId).SendAsync("AddedNewComment", comment);
        }
        public async Task AutoAssigner(Ticket newTicket)
        {
            if (Tickets == null) Tickets = new List<Ticket>();
            Tickets.Add(newTicket);
            var lowWorkloadSpecialist = Specialists.OrderBy(x => x.Value.Workload).FirstOrDefault();
            if(lowWorkloadSpecialist.Value != null)
            {
                if (lowWorkloadSpecialist.Value.AssignedTickets is null) 
                    lowWorkloadSpecialist.Value.AssignedTickets = new List<Ticket>();
                
                newTicket.AssignedSpecialist = lowWorkloadSpecialist.Value;
                lowWorkloadSpecialist.Value.AssignedTickets.Add(newTicket);
                await specialistHub.Clients.Group(lowWorkloadSpecialist.Key)
                                           .SendAsync("ReceiveSpecialistAssigned", newTicket);
            }
        }
    }
}
