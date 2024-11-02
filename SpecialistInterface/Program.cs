using Schemas.Entities;
using Microsoft.AspNetCore.SignalR.Client;
using System.Text;
using System.Net.Sockets;

var hubUrl = $"https://localhost:7183/specialisthub?userId={GenerateRandomString(10)}";
Specialist specialistProfile = null;

var connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .Build();


connection.On<Specialist>("ReceiveSpecialistProfile", specialist =>
{
    specialistProfile = specialist;
    Console.WriteLine($"\r\nYou are log in: \r\nUsername: {specialist.Name}\r\nEmail: {specialist.Email}\r\nUser ID: {specialist.UserId}");
});

connection.On<Ticket>("ReceiveSpecialistAssigned", ticket =>
{
    Console.WriteLine($"Ticket #{ticket.Id} assigned for you");
    specialistProfile.AssignedTickets.Add(ticket);
});


try
{
    await connection.StartAsync();
    Console.WriteLine("Connected to SpecialistHub.");

    Console.WriteLine("Commands:");
    Console.WriteLine("1. Push 'Insert' for creating comment for ticket");
    Console.WriteLine("2. Push 'Enter' for exit...");


    while (true)
    {
        var key = Console.ReadKey(true);

        if (key.Key == ConsoleKey.Insert)
        {
            if(specialistProfile.AssignedTickets.Count > 0)
            {
                Console.WriteLine("Tickets list is empty");
            }

            Console.WriteLine("Type ticket number for adding comment: ");
            foreach(var ticket in specialistProfile.AssignedTickets)
            {
                Console.WriteLine($"{ticket.Id}. {ticket.Title}");
            }

            var number = Console.ReadLine();
            if(Int32.TryParse(number, out var id)) 
            {
                var ticket = specialistProfile.AssignedTickets.FirstOrDefault(x => x.Id == id);
                if(ticket != null)
                {
                    ticket.AddComment(new Comment
                    {
                        Id = ticket.Comments.Count + 1,
                        CreatedAt = DateTime.Now,
                        CreatedBy = specialistProfile,
                        Text = $"Comment {ticket.Comments.Count + 1}",
                        Ticket = ticket
                    }, $"https://localhost:7183/api/tickets/{id}");
                    Console.WriteLine($"Added new comment for ticket # {ticket.Id}");
                }
                continue;
            }

            Console.WriteLine("Wrong id");
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