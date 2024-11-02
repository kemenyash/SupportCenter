using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schemas.Entities
{
    public class Specialist : User
    {
        public int Workload { get; set; }
        public List<Ticket> AssignedTickets { get; set; }
    }
}
