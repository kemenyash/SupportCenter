using Microsoft.AspNetCore.SignalR.Client;
using Schemas.Entities;
using System.Text;
using System.Xml.Linq;

var hubUrl = $"https://localhost:7183/customerhub?userId={GenerateRandomString(10)}";
int ticketsId = 0;
Customer customerProfile = null;

var connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();

connection.On<Comment>("AddedNewComment", comment =>
{
    Console.WriteLine($"Added new comment: {comment.Text} \r\nTicket ID: {comment.Ticket.Id}\r\nSpecialist: {comment.Ticket.AssignedSpecialist.Name}");
    var ticket = customerProfile.Tickets.FirstOrDefault(x => x.Id == comment.Ticket.Id);
    ticket.Comments.Add(comment);
});

connection.On<Customer>("ReceiveCustomerProfile", customer =>
{
    customerProfile = customer;
    Console.WriteLine($"\r\nYou are log in: \r\nUsername: {customer.Name}\r\nEmail: {customer.Email}\r\nUser ID: {customer.UserId}");
});
try
{
    await connection.StartAsync();
    Console.WriteLine("Connected to CustomerHub.");
    Console.WriteLine("Commands:");
    Console.WriteLine("1. Push 'Insert' for creating ticket");
    Console.WriteLine("2. Push 'Enter' for exit...");


    while (true)
    {
        var key = Console.ReadKey(true);

        if (key.Key == ConsoleKey.Insert)
        {
            ticketsId++;
            await customerProfile.CreateTicket("https://localhost:7183/api/tickets", new Ticket
            {
                CreatedAt = DateTime.Now,
                CreatedBy = customerProfile,
                Description = "Some description",
                Title = $"Some problem #{ticketsId}",
                Priority = Schemas.Enums.PriorityLevel.Medium,
                Id = ticketsId,
                Status = Schemas.Enums.TicketStatus.Open
            });
            Console.WriteLine($"Ticket #{ticketsId} created");
        }
        else if (key.Key == ConsoleKey.Enter)
        {
            Console.WriteLine("Exiting...");
            break;
        }
        else
        {
            Console.WriteLine("Invalid command. Try again.");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Connection error: {ex.Message}");
}
finally
{
    await connection.DisposeAsync();
}


static string GenerateRandomString(int length)
{
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    var random = new Random();
    var result = new StringBuilder(length);

    for (int i = 0; i < length; i++)
    {
        result.Append(chars[random.Next(chars.Length)]);
    }

    return result.ToString();
}
