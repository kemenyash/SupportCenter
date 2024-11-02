using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schemas.Entities
{
    public class Report
    {
        public string Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Content { get; set; }
    }
}
