using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Schemas.Entities;
using TicketsHub.Services;

namespace TicketsHub.Controllers
{

    [Route("api/[controller]")]
    [ApiController]

    public class TicketsController : ControllerBase
    {
        private readonly TicketsProcesser ticketsProcesser;

        public TicketsController(TicketsProcesser ticketsProcesser)
        {
            this.ticketsProcesser = ticketsProcesser;
        }

        [HttpGet]
        public async Task<IActionResult> GetTickets()
        {
            return Ok(TicketsProcesser.Tickets ?? new List<Ticket>());
        }

        [HttpPost]
        [Route(("{id}"))]
        public async Task<IActionResult> CreateCommentForTicket([FromBody] Comment comment)
        {
            await ticketsProcesser.AddNewComment(comment);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody]Ticket ticket)
        {
            await ticketsProcesser.AutoAssigner(ticket);
            return Ok();
        }
    }
}
